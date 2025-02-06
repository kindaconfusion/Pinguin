using System.Collections.ObjectModel;
using System.Net;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Windowing;
using Moq;
using Pinguin.Models;
using Pinguin.ViewModels;
using Pinguin.Views;

namespace Pinguin.Test.Views;

[TestFixture]
public class MainViewTest
{
    private Mock<MainViewModel> _mock;
    private MainView appWindow;
    
    [SetUp]
    public void Setup()
    {
        var mockRunner = new Mock<IPingRunner>();
        mockRunner.SetupProperty(p => p.Pings,
            new ObservableCollection<PingObject> {new() {IpAddress = IPAddress.Parse("1.1.1.1")}});
        _mock = new(mockRunner.Object);
        appWindow = new MainView();
        appWindow.DataContext = _mock.Object;
        appWindow.Show();
    }
    
    [AvaloniaTest]
    public void ContextMenu_Test()
    {
        Console.WriteLine((appWindow.DataContext as MainViewModel).Pings[0].IpAddress + " cuck");
        PingObject ping = new PingObject() {IpAddress = IPAddress.Parse("1.1.1.1")};
        var row = appWindow.PingGrid.GetVisualChildren()
            .OfType<DataGridRow>()
            .FirstOrDefault(r => (r.DataContext as PingObject).Equals(ping));
        Console.WriteLine(row.Header.ToString());
    }
}