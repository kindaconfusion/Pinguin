using System.Collections.ObjectModel;
using System.Net;
using Moq;
using Pinguin.Models;
using Pinguin.ViewModels;

namespace Pinguin.Test.ViewModels;

[TestFixture]
public class MainVmTest
{
    private MainViewModel _vm;
    private Mock<IPingRunner> _mock;
    private ObservableCollection<PingObject> _pings;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IPingRunner>();
        _pings = new ObservableCollection<PingObject>();
        _mock.Setup(p => p.Pings).Returns(_pings);
        _mock.Setup(p => p.RemovePing(new PingObject{IpAddress = IPAddress.Parse("127.0.0.1")}))
            .Callback(() => _mock.Object.Pings.Remove(new PingObject{IpAddress = IPAddress.Parse("127.0.0.1")}));

        
        _vm = new MainViewModel(_mock.Object);
        
    }

    [Test]
    public void RemovePing_ValidPing_Removes()
    {
        _pings.Add(new PingObject{IpAddress = IPAddress.Parse("127.0.0.1")});
        _vm.SelectedIndex = 0;
        _vm.DeletePing();
        if (_mock.Object.Pings.Count > 0) Assert.Fail();
    }
    
    [Test]
    public void RemovePing_InvalidPing_DoesNothing()
    {
        _pings.Add(new PingObject{IpAddress = IPAddress.Parse("127.0.0.1")});
        _vm.SelectedIndex = 1;
        _vm.DeletePing();
        if (_mock.Object.Pings.Count == 0) Assert.Fail();
    }
}