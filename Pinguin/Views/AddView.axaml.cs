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

public partial class AddView : ContentDialog
{
    protected override Type StyleKeyOverride => typeof(ContentDialog);
    public AddView()
    {
        InitializeComponent();
        DataContext = ServiceLocator.Instance.GetService(typeof(AddViewModel));
    }
}