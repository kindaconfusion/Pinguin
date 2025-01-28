
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using LiveChartsCore.SkiaSharpView;
using Pinguin.Models;
using Pinguin.ViewModels;


namespace Pinguin.Views;

public partial class MainView : AppWindow
{

    public MainView()
    {
        InitializeComponent();
        /*DaChart.XAxes = new List<Axis>{new Axis {MinStep = 1}};
        DaChart.YAxes = new List<Axis> {new Axis
        { MinStep = 1 }};*/
    }

    private void AddButtonOnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddView();
        dialog.ShowAsync();
    }
    private void OptionsButtonOnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new PingOptionsView();
        dialog.ShowAsync();
    }
    private void PingGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        Point position;
        e.TryGetPosition(sender as DataGrid, out position);
        var hitTestResult = PingGrid.InputHitTest(position);
        if (hitTestResult is Visual visualElement)
        {
            var dataGridRow = visualElement.GetVisualAncestors()
                .OfType<DataGridRow>()
                .FirstOrDefault();
            var context = DataContext as MainViewModel;
            if (dataGridRow != null)
            {
                var flyout = new MenuFlyout
                {
                    Items =
                    {
                        new MenuItem {Header = "Remove", Command = context.DeletePingCommand},
                        new MenuItem {Header = "Open Graph", 
                            Command = context.OpenGraphCommand, 
                            CommandParameter = dataGridRow.DataContext as PingObject }
                    },
                };
                flyout.ShowAt(sender as Control, true);
            }
        }
        e.Handled = true;
    }
}