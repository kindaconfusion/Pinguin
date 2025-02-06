using System.Net;
using Pinguin.Models;

namespace Pinguin.Test;

[TestFixture]
public class PingTest
{
    private PingRunner _pingRunner;

    [SetUp]
    public void Setup()
    {
        _pingRunner = new PingRunner();
    }
    
    [Test]
    public async Task ResolveHostName_ValidIP_Resolves()
    {
        var hostname = await PingRunner.ResolveHostName(IPAddress.Parse("1.1.1.1"));
        Console.WriteLine(hostname);
        if (hostname.Equals("one.one.one.one")) Assert.Pass(); else Assert.Fail();
    }
    
    [Test]
    public async Task ResolveIP_ValidHostname_Resolves()
    {
        var ip = await PingRunner.ResolveIp("one.one.one.one");
        if (ip.ToString().Equals("1.1.1.1") || ip.ToString().Equals("1.0.0.1")) Assert.Pass(); else Assert.Fail();
    }

    [Test]
    public async Task RunPing_ValidHost_CompletesPings()
    {
        _pingRunner.Settings = new Options()
        {
            Interval = 0.1
        };
        await _pingRunner.AddPing("8.8.8.8");
        var ping = _pingRunner.Pings[0];
        var cts = _pingRunner.Tasks[ping];
        while (ping.PingsSent < 25)
        {
            await Task.Delay(100);
        }
        cts.Cancel();
        Console.WriteLine(ping.PingPercent);
        if (ping.PingPercent < .25) Assert.Pass();
        else Assert.Fail();
    }
    

}