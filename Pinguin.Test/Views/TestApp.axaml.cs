using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pinguin.Test.Views;

public class TestApp : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

