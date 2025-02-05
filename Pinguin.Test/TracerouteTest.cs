using System.Diagnostics;
using NUnit.Framework;
using System.Net;
using Pinguin.Models;

namespace Pinguin.Test;

[TestFixture]
public class Tests
{
    
    [Test]
    public async Task TraceRouteTest()
    {
        var ping = new PingObject()
        {
            IpAddress = IPAddress.Parse("8.8.8.8"),
        };
        try
        {
            await foreach (var p in Traceroute.RunTraceroute(ping))
            {
                Console.WriteLine(p.IpAddress.ToString());
                if (p.IpAddress.Equals(IPAddress.Parse("8.8.8.8"))) Assert.Pass();
            }
        }
        catch (Exception)
        {
            Assert.Fail();
        }
        
    }
}