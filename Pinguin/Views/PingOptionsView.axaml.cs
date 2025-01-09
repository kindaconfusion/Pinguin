using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DialogHostAvalonia;

namespace Pinguin.Views;

public partial class PingOptionsView : UserControl
{
    public PingOptionsView()
    {
        InitializeComponent();
        /*DataContextChanged += (sender, args) =>
        {
            if (DataContext is ICloseable closeable) closeable.Closed += (s, e) => Close();
        };*/
    }
    


}