using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using Pinguin.Services;
using Pinguin.ViewModels;

namespace Pinguin.Views;

public partial class PingOptionsView : ContentDialog
{
    protected override Type StyleKeyOverride => typeof(ContentDialog);
    public PingOptionsView()
    {
        InitializeComponent();
        DataContext = ServiceLocator.Instance.GetService(typeof(PingOptionsViewModel));
    }
    


}