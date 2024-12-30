using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.VisualTree;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Pinguin;

namespace Pinguin.Services;

public static class WindowHelper
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public static async Task OpenDialogAsync<TViewModel>(this object? context, bool isDialog = true) where TViewModel : ObservableObject
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
                window.ShowDialog(topLevel.GetVisualRoot() as Window);
            else
                window.Show(topLevel.GetVisualRoot() as Window);
        }

        
    }
}