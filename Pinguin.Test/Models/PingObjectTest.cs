using System.Net;
using Pinguin.Models;

namespace Pinguin.Test.Models;

[TestFixture]
public class PingObjectTest
{
    [Test]
    public void Equals_SameIps_True()
    {
        var ping1 = new PingObject
        {
            IpAddress = IPAddress.Parse("127.0.0.1"),
            // just some garbage data
            PingPercent = 1111,
            AveragePing = 69
        };
        var ping2 = new PingObject
        {
            IpAddress = IPAddress.Parse("127.0.0.1"),
            PingPercent = 1237,
            AveragePing = 6
        };
        if (!ping1.Equals(ping2)) Assert.Fail();
    }

    [Test]
    public void Equals_DifferentIps_False()
    {
        var ping1 = new PingObject
        {
            IpAddress = IPAddress.Parse("127.0.0.1"),
            PingPercent = 1111,
            AveragePing = 69
        };
        var ping2 = new PingObject
        {
            IpAddress = IPAddress.Parse("1.1.1.1"),
            PingPercent = 1234,
            AveragePing = 6
        };
        if (ping1.Equals(ping2)) Assert.Fail();
    }

    [Test]
    public void Equals_SameHostname_True()
    {
        var ping1 = new PingObject
        {
            HostName = "localhost",
            // just some garbage data
            PingPercent = 1111,
            AveragePing = 69
        };
        var ping2 = new PingObject
        {
            HostName = "localhost",
            PingPercent = 1234,
            AveragePing = 6
        };
        if (!ping1.Equals(ping2)) Assert.Fail();
    }

    [Test]
    public void Equals_DifferentHostname_False()
    {
        var ping1 = new PingObject
        {
            HostName = "localhost",
            PingPercent = 1111,
            AveragePing = 69
        };
        var ping2 = new PingObject
        {
            HostName = "google.com",
            PingPercent = 1234,
            AveragePing = 6
        };
        if (ping1.Equals(ping2)) Assert.Fail();
    }
}