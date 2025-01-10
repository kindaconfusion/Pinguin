using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Pinguin.Services;
using Pinguin.ViewModels;

namespace Pinguin.Views;

public partial class AddView : ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContentDialog);
    public AddView()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = ServiceLocator.Instance.GetService(typeof(AddViewModel));
    }
}