using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pinguin.Views;

public partial class AddView : UserControl
{
    public AddView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}