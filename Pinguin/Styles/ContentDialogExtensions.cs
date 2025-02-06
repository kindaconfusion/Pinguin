using Avalonia;
using FluentAvalonia.UI.Controls;

namespace Pinguin.Styles;

public static class ContentDialogExtensions
{
    public static readonly AttachedProperty<bool> IsCloseButtonEnabledProperty =
        AvaloniaProperty.RegisterAttached<ContentDialog, bool>(
            "IsCloseButtonEnabled",
            typeof(ContentDialogExtensions),
            true);

    public static void SetIsCloseButtonEnabled(ContentDialog element, bool value)
    {
        element.SetValue(IsCloseButtonEnabledProperty, value);
    }

    public static bool GetIsCloseButtonEnabled(ContentDialog element)
    {
        return element.GetValue(IsCloseButtonEnabledProperty);
    }
}