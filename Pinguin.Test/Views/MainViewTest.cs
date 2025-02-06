using System.Collections.ObjectModel;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Windowing;
using LiveChartsCore.Measure;
using Moq;
using Pinguin.Models;
using Pinguin.ViewModels;
using Pinguin.Views;

namespace Pinguin.Test.Views;

[TestFixture]
public class MainViewTest
{
    private Mock<MainViewModel> _mock;
    private ObservableCollection<PingObject> fuck;
    private MainView appWindow;
    
    [SetUp]
    public void Setup()
    {
        var mockRunner = new Mock<IPingRunner>();
        fuck =  new ObservableCollection<PingObject>();
        mockRunner.Setup(p => p.Pings).Returns(fuck);
        _mock = new(mockRunner.Object);
        appWindow = new MainView();
        appWindow.DataContext = _mock.Object;
        appWindow.Show();
    }
    
    
    
}