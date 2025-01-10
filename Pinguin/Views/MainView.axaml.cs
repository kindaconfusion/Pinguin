
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Pinguin.Models;
using Pinguin.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;

namespace Pinguin.Views;

public partial class MainView : AppWindow
{

    public MainView()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddView();
        dialog.ShowAsync();
    }
}