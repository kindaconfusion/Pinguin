using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Pinguin.Models;

public class PingRunner
{
    public RangeObservableCollection<PingObject> Pings { get; set; } = new RangeObservableCollection<PingObject>();
    public PingRunner(IEnumerable<PingObject>? pings)
    {
        if (pings != null)
            Pings.ReplaceRange(pings);
    }

    public void ReplacePings(IEnumerable<PingObject> pings)
    {
        Pings.ReplaceRange(pings);
        Task[] threads = new Task[Pings.Count];
        for (int i = 0; i < Pings.Count; i++)
        {
            int index = i;
            threads[i] = Task.Run(() => RunPing(index, Dispatcher.UIThread));
        }
    }

    private async Task RunPing(int index, Dispatcher dispatcher)
    {
        await ResolveIp(index, dispatcher);
        var ping = Pings[index];
        while (true)
        {
            await Task.Delay(1000);
            using Ping p = new Ping();
            ping.PingsSent++;
            var reply = await p.SendPingAsync(ping.IpAddress);
            ping.AddReply(reply);
            dispatcher.Invoke(() =>
            {
                Pings[index] = ping;
                Pings.NotifyChanges();
            });
        }
    }
    
    public async Task ResolveIp(int index, Dispatcher dispatcher)
    {
        var ping = Pings[index];
        ping.HostName.Trim().TrimEnd('\r', '\n');
        try 
        {
            if (!IPAddress.TryParse(ping.HostName, out IPAddress address))
            {
                var entry = await Dns.GetHostEntryAsync(ping.HostName);

                if (entry.AddressList.Length == 0)
                {
                    return;
                }
                ping.IpAddress = entry.AddressList[0];
            }
            else
            {
                ping.IpAddress = address;
            }
            await dispatcher.InvokeAsync(() =>
            {
                Pings[index] = ping;
                Pings.NotifyChanges();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Thread {index}: Error occurred - {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}