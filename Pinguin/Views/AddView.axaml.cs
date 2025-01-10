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
        AvaloniaXamlLoader.Load(this);
        DataContext = ServiceLocator.Instance.GetService(typeof(AddViewModel));
        IsPrimaryButtonEnabled = false;
        IsSecondaryButtonEnabled = false;
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            IsPrimaryButtonEnabled = !String.IsNullOrEmpty(textBox.Text);
            IsSecondaryButtonEnabled = !String.IsNullOrEmpty(textBox.Text);
        }
        
    }
}