using Microsoft.Extensions.DependencyInjection;
using Pinguin.Models;
using Pinguin.ViewModels;

namespace Pinguin.Services;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddTransient<MainViewModel>();
        collection.AddSingleton<PingOptionsViewModel>();
        collection.AddSingleton<PingRunner>();
        collection.AddTransient<AddViewModel>();
    }
}