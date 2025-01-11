using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pinguin.Models;

public static class Traceroute
{
    public async static IAsyncEnumerable<PingObject> RunTraceroute(PingObject host)
    {
        var traceroute = new List<PingObject>();
        var options = new PingOptions();
        options.Ttl = 1;
        while (traceroute.Count == 0 || !traceroute.Last().IpAddress.Equals(host.IpAddress))
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var sender = new Ping();
            var reply = await sender.SendPingAsync(host.IpAddress, new TimeSpan(0, 0, 1), Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), options, token);
            if (reply.Status is IPStatus.Success or IPStatus.TtlExpired)
            {
                var ping = new PingObject { IpAddress = host.IpAddress };
                try
                {
                    var hostname = Dns.GetHostEntryAsync(reply.Address);
                    var timeoutTask = Task.Delay(1000);
                    
                    if (await Task.WhenAny(hostname, timeoutTask) == hostname)
                    {
                        ping.HostName = hostname.Result.HostName;
                    }
                    else
                    {
                        ping.HostName = reply.Address.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while tracing - {ex.GetType().Name}: {ex.Message}");
                }
                
                traceroute.Add(ping);
                yield return ping;
                if (reply.Status == IPStatus.Success) break;
            }
            options.Ttl++;
        }
    }
}