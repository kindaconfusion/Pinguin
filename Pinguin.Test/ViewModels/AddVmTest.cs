using Moq;
using Pinguin.Models;
using Pinguin.ViewModels;

namespace Pinguin.Test.ViewModels;

[TestFixture]
public class AddVmTest
{
    private AddViewModel _vm;
    private Mock<IPingRunner> _mock;

    [SetUp]
    public void Setup()
    {
        _mock = new Mock<IPingRunner>();
        _mock.Setup(x => x.Tracert("1.1.1.1"))
            .Returns(Task.Delay(10000));

        _vm = new AddViewModel(_mock.Object);
        
    }

    [Test]
    public async Task During_AddTraceRoute_ButtonsUnclickable()
    {
        _vm.HostName = "1.1.1.1";
        _vm.AddTraceRoute();
        if (_vm.CloseCommand.CanExecute(null) || _vm.AddHostCommand.CanExecute(null) || _vm.AddTraceRouteCommand.CanExecute(null)) Assert.Fail();
    }

    [Test]
    public void HostNameEmpty_CanAdd_False()
    {
        _vm.HostName = "garbage data";
        _vm.HostName = string.Empty;
        if (_vm.AddHostCommand.CanExecute(null) || _vm.AddTraceRouteCommand.CanExecute(null)) Assert.Fail();
    }
    
    [Test]
    public void HostNameNotEmpty_CanAdd_True()
    {
        _vm.HostName = string.Empty;
        _vm.HostName = "garbage data";
        if (!_vm.AddHostCommand.CanExecute(null) || !_vm.AddTraceRouteCommand.CanExecute(null)) Assert.Fail();
    }
}