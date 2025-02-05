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
        while (options.Ttl < 31 && (traceroute.Count == 0 || !traceroute.Last().IpAddress.Equals(host.IpAddress)))
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var sender = new Ping();
            var reply = await sender.SendPingAsync(host.IpAddress, new TimeSpan(0, 0, 0, 0, 500), null, options, token);
            if (reply.Status is IPStatus.Success or IPStatus.TtlExpired or IPStatus.TimeExceeded)
            {
                var ping = new PingObject { IpAddress = reply.Address };
                try
                {
                    ping.HostName = await PingRunner.ResolveHostName(reply.Address);
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
        if (options.Ttl == 30)
        {
            throw new TimeoutException("Unable to reach host after 30 hops.");
        }
    }
}