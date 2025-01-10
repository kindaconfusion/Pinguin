using System.Diagnostics;
using NUnit.Framework;
using System.Net;
using Pinguin.Models;

namespace Pinguin.Test;

[TestFixture]
public class Tests
{

    [Test]
    public void Test()
    {
        Console.WriteLine("huh");
        Assert.Pass();
    }
    
    /*[Test]
    public async Task Test1()
    {
        var SHIT = new PingObject(IPAddress.Parse("8.8.8.8"));
        var trace = await Traceroute.RunTraceroute(SHIT);
        TestContext.Out.WriteLine(String.Join(", ", trace.Select(x => x.IpAddress)));
        Assert.Pass();
    }*/
}