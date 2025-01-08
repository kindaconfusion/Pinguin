using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Pinguin;
using Pinguin.ViewModels;

namespace Pinguin.Services;

[Obsolete]
public static class WindowHelper
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public static async Task<ViewModelBase?>? OpenDialogAsync<TViewModel>(this object? context, bool isDialog = true) where TViewModel : ObservableObject
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        var viewModel = ServiceProvider.GetRequiredService<TViewModel>();

        // lookup the TopLevel for the context
        var topLevel = WindowManager.GetVisualForContext(context);
        var viewLocator = new ViewLocator();
        var view = viewLocator.Build(typeof(TViewModel));

        if (view is Window window)
        {
            window.DataContext = viewModel;
            if (isDialog)
            {
                await window.ShowDialog(topLevel.GetVisualRoot() as Window);
                return window.DataContext as ViewModelBase;
            }
            else
                window.Show(topLevel.GetVisualRoot() as Window);
        }
        return null;
    }
}