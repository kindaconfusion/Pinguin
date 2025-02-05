using System.Net;
using Pinguin.Models;

namespace Pinguin.Test;

[TestFixture]
public class PingTest
{
    [Test]
    public async Task ResolveHostName_ValidIP_Resolves()
    {
        var hostname = await PingRunner.ResolveHostName(IPAddress.Parse("1.1.1.1"));
        if (hostname.Equals("one.one.one.one")) Assert.Pass();
    }
    
    [Test]
    public async Task ResolveIP_ValidHostname_Resolves()
    {
        var ip = await PingRunner.ResolveIp("one.one.one.one");
        if (ip.Equals("1.1.1.1")) Assert.Pass();
    }
}