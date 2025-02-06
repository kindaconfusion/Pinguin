using Avalonia;
using Avalonia.Headless;
using Pinguin.Test.Views;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace Pinguin.Test.Views;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseSkia()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}