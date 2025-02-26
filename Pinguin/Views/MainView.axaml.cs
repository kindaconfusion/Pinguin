using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Pinguin.Models;
using Pinguin.ViewModels;

namespace Pinguin.Views;

public partial class MainView : AppWindow
{
    public MainView()
    {
        InitializeComponent();
        TitleBar.ButtonInactiveForegroundColor = ActualThemeVariant == ThemeVariant.Light ? Colors.Black : Colors.White;
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
                        new MenuItem
                        {
                            Header = "Open Graph",
                            Command = context.OpenGraphCommand,
                            CommandParameter = dataGridRow.DataContext as PingObject
                        }
                    }
                };
                flyout.ShowAt(sender as Control, true);
            }
        }

        e.Handled = true;
    }

    private void TeachingTip_OnActionButtonClick(TeachingTip sender, EventArgs args)
    {
        // pain in my ass
        var url = "https://github.com/kindaconfusion/Pinguin/releases";
        try
        {
            Process.Start(url);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) {UseShellExecute = true});
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }

        Tip.IsOpen = false;
    }
}