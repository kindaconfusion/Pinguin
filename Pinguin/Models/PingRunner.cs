using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Pinguin.Models;

public class PingRunner
{
    public Options Settings { get; set; } = new ();
    public ObservableCollection<PingObject> Pings { get; set; } = new();
    
    public Dictionary<PingObject, CancellationTokenSource> Tasks = new();
    public PingRunner(IEnumerable<PingObject>? pings)
    {
        //if (pings != null)
            //Pings.ReplaceRange(pings);
    }

    /*[Obsolete("Use AddPing instead, for now...")]
    public void ReplacePings(IEnumerable<PingObject> pings)
    {
        Pings.ReplaceRange(pings);
        Task[] threads = new Task[Pings.Count];
        for (int i = 0; i < Pings.Count; i++)
        {
            int index = i;
            //threads[i] = Task.Run(() => RunPing(index, Dispatcher.UIThread));
        }
    }*/

    public async Task Tracert(string host)
    {
        var ping = new PingObject {HostName = host};
        ping.IpAddress = await ResolveIp(host);
        await foreach (var p in Traceroute.RunTraceroute(ping))
        {
            AddPing(p);
        }
    }

    public async Task AddPing(string host)
    {
        IPAddress? ip = null;
        IPAddress.TryParse(host, out ip);
        var ping = new PingObject
        {
            HostName = (ip is not null) ? await ResolveHostName(ip) : host,
            IpAddress = ip ?? await ResolveIp(host)
        };
        
        Pings.Add(ping);
        var cts = new CancellationTokenSource();
        Tasks.Add(ping, cts);
        Task.Run(() => RunPing(ping, cts.Token));
    }
    
    public async void AddPing(PingObject ping)
    {
        Pings.Add(ping);
        var cts = new CancellationTokenSource();
        Tasks.Add(ping, cts);
        Task.Run(() => RunPing(ping, cts.Token));
    }

    public async void RemovePing(PingObject ping)
    {
        CancellationTokenSource token;
        if (!Tasks.TryGetValue(ping, out token)) return;
        token.Cancel();
        Tasks.Remove(ping);
        Pings.Remove(ping);
    }

    private async Task RunPing(PingObject ping, CancellationToken cancel)
    {
        while (true)
        {
            if (cancel.IsCancellationRequested)
            {
                Console.WriteLine("Stopping ping.");
                cancel.ThrowIfCancellationRequested();
            }
            //var ping = Pings.FirstOrDefault(p => p.IpAddress.Equals(inPing.IpAddress));
            await Task.Delay((int) (Settings.Interval * 1000.0));
            using Ping p = new Ping();
            ping.PingsSent++;
            PingReply reply;
            try
            {
                ping.AddReply(await p.SendPingAsync(ping.IpAddress, Settings.Timeout * 1000 ?? 2000,
                    Encoding.ASCII.GetBytes(GeneratePingContent(Settings.PacketSize.Value))));
            }
            catch (PlatformNotSupportedException)
            {
                reply = await p.SendPingAsync(ping.IpAddress, (Settings.Timeout * 1000) ?? 2000);
                ping.AddReply(reply);
            }
        }
    }
    
    public static async Task<IPAddress?> ResolveIp(string host)
    {
        host.Trim().TrimEnd('\r', '\n');
        if (!IPAddress.TryParse(host, out IPAddress? address))
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
    
    private string GeneratePingContent(int length)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (length <= alphabet.Length)
        {
            // If length is less than or equal to 26, truncate the alphabet
            return alphabet.Substring(0, length);
        }
        else
        {
            // If length is greater than 26, repeat and truncate
            return string.Concat(Enumerable.Repeat(alphabet, (length + alphabet.Length - 1) / alphabet.Length))
                .Substring(0, length);
        }
    }

    public static async Task<string> ResolveHostName(IPAddress address)
    {
        string hostEntry;
        var hostname = Dns.GetHostEntryAsync(address);
        var timeoutTask = Task.Delay(1000);
                    
        if (await Task.WhenAny(hostname, timeoutTask) == hostname)
        {
            hostEntry = hostname.Result.HostName;
        }
        else
        {
            hostEntry = address.ToString();
        }
        return hostEntry;
    }
}