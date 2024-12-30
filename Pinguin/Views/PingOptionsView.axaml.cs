using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Pinguin.Views;

public partial class PingOptionsView : Window
{
    public PingOptionsView()
    {
        InitializeComponent();
        DataContextChanged += (sender, args) =>
        {
            if (DataContext is ICloseable closeable) closeable.Closed += (s, e) => Close();
        };
    }
    

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}