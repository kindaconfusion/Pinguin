using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Pinguin.Services;
using Pinguin.ViewModels;

namespace Pinguin.Views;

public partial class PingOptionsView : ContentDialog
{
    public PingOptionsView()
    {
        InitializeComponent();
        DataContext = ServiceLocator.Instance.GetService(typeof(PingOptionsViewModel));
        IsPrimaryButtonEnabled = false;
        IsSecondaryButtonEnabled = false;
    }

    protected override Type StyleKeyOverride => typeof(ContentDialog);

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox) IsPrimaryButtonEnabled = (DataContext as PingOptionsViewModel).CanSave();
    }
}