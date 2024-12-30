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
    public async static Task<List<PingObject>> RunTraceroute(PingObject host)
    {
        var traceroute = new List<PingObject>();
        var options = new PingOptions();
        options.Ttl = 1;
        while (traceroute.Count == 0 || !traceroute.Last().IpAddress.Equals(host.IpAddress))
        {
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            var sender = new Ping();
            var reply = await sender.SendPingAsync(host.IpAddress, new TimeSpan(0, 0, 5), Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"), options, token);
            if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
                traceroute.Add(new PingObject(reply.Address));
            
            //Console.WriteLine(reply.Address);
            options.Ttl++;
        }
        return traceroute;
    }
}