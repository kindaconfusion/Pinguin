using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FluentAvalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Pinguin.Services;
using Pinguin.ViewModels;
using Pinguin.Views;

namespace Pinguin;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var collection = new ServiceCollection();
            collection.AddCommonServices();


            // Creates a ServiceProvider containing services from the provided IServiceCollection
            var services = collection.BuildServiceProvider();
            ServiceLocator.Instance = services; // bad practice? suck my DICK AND BALLS
            var faTheme = Current?.Styles.OfType<FluentAvaloniaTheme>().FirstOrDefault();
            faTheme.PreferUserAccentColor = true;
            DisableAvaloniaDataAnnotationValidation();
            var vm = services.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
    }
}