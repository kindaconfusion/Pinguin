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
    public Options Settings { get; set; } = new Options(1, 32);
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

    public async Task Tracert(string host)
    {
        //var end = Pings.Count;
        //Pings.Insert(end, new PingObject(host));
        var ping = new PingObject(host);
        ping.IpAddress = await ResolveIp(host);
        //var trace = await Traceroute.RunTraceroute(ping);
        await foreach (var p in Traceroute.RunTraceroute(ping))
        {
            AddPing(p);
        }
    }

    public async Task AddPing(string host)
    {
        var end = Pings.Count;
        var ip = await ResolveIp(host);
        var ping = new PingObject(host);
        ping.IpAddress = ip;
        Pings.Insert(end, ping);
        Task thread = Task.Run(() => RunPing(end, Dispatcher.UIThread));
    }
    public async void AddPing(PingObject ping)
    {
        var end = Pings.Count;
        Pings.Insert(end, ping);
        Task thread = Task.Run(() => RunPing(end, Dispatcher.UIThread));
    }

    private async Task RunPing(int index, Dispatcher dispatcher)
    {
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
    
    public async Task<IPAddress?> ResolveIp(string host)
    {
        host.Trim().TrimEnd('\r', '\n');
        try 
        {
            if (!IPAddress.TryParse(host, out IPAddress address))
            {
                var entry = await Dns.GetHostEntryAsync(host);

                if (entry.AddressList.Length == 0)
                {
                    return null;
                }
                return entry.AddressList[0];
            }
            return address;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while resolving IP - {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}