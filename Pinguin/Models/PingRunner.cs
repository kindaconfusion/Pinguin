using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Pinguin.Models;

public class PingRunner
{
    public Options Settings { get; set; } = new Options(1, 32);
    public RangeObservableCollection<PingObject> Pings { get; set; } = new();
    
    private Dictionary<PingObject, CancellationTokenSource> _tasks = new();
    public PingRunner(IEnumerable<PingObject>? pings)
    {
        if (pings != null)
            Pings.ReplaceRange(pings);
    }

    [Obsolete("Use AddPing instead, for now...")]
    public void ReplacePings(IEnumerable<PingObject> pings)
    {
        Pings.ReplaceRange(pings);
        Task[] threads = new Task[Pings.Count];
        for (int i = 0; i < Pings.Count; i++)
        {
            int index = i;
            //threads[i] = Task.Run(() => RunPing(index, Dispatcher.UIThread));
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
        var cts = new CancellationTokenSource();
        _tasks.Add(ping, cts);
        Task.Run(() => RunPing(ping, cts.Token, Dispatcher.UIThread));
    }
    
    public async void AddPing(PingObject ping)
    {
        var end = Pings.Count;
        Pings.Insert(end, ping);
        var cts = new CancellationTokenSource();
        _tasks.Add(ping, cts);
        Task.Run(() => RunPing(ping, cts.Token, Dispatcher.UIThread));
    }

    public async void RemovePing(PingObject ping)
    {
        CancellationTokenSource token;
        if (!_tasks.TryGetValue(ping, out token)) Console.WriteLine("Fuck");
        token.Cancel();
        _tasks.Remove(ping);
        Pings.Remove(ping);
    }

    private async Task RunPing(PingObject inPing, CancellationToken cancel, Dispatcher dispatcher)
    {
        while (true)
        {
            if (cancel.IsCancellationRequested)
            {
                Console.WriteLine("Stopping ping.");
                cancel.ThrowIfCancellationRequested();
            }
            var ping = Pings.FirstOrDefault(p => p.IpAddress.Equals(inPing.IpAddress));
            await Task.Delay(1000);
            using Ping p = new Ping();
            ping.PingsSent++;
            var reply = await p.SendPingAsync(ping.IpAddress);
            ping.AddReply(reply);
            dispatcher.Invoke(() =>
            {
                //Pings[index] = ping;
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