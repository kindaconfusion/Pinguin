using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Pinguin.Behaviors;

public class NumericInputBehavior : Behavior<TextBox>
{
    public static readonly StyledProperty<bool> AllowDecimalProperty =
        AvaloniaProperty.Register<NumericInputBehavior, bool>(
            nameof(AllowDecimal),
            true);

    public static readonly StyledProperty<bool> AllowNegativeProperty =
        AvaloniaProperty.Register<NumericInputBehavior, bool>(
            nameof(AllowNegative),
            true);

    public bool AllowDecimal
    {
        get => GetValue(AllowDecimalProperty);
        set => SetValue(AllowDecimalProperty, value);
    }

    public bool AllowNegative
    {
        get => GetValue(AllowNegativeProperty);
        set => SetValue(AllowNegativeProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            AssociatedObject.KeyDown += OnKeyDown;
            AssociatedObject.TextInput += OnTextInput;
            AssociatedObject.TextChanged += OnTextChanged;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
        {
            AssociatedObject.KeyDown -= OnKeyDown;
            AssociatedObject.TextInput -= OnTextInput;
            AssociatedObject.TextChanged -= OnTextChanged;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Allow control keys
        if (e.Key == Key.Back || e.Key == Key.Delete ||
            e.Key == Key.Left || e.Key == Key.Right ||
            e.Key == Key.Tab)
            return;

        // Block space key
        if (e.Key == Key.Space)
        {
            e.Handled = true;
            return;
        }

        // Handle negative sign
        if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            if (!AllowNegative || AssociatedObject?.CaretIndex != 0)
                e.Handled = true;
    }

    private void OnTextInput(object sender, TextInputEventArgs e)
    {
        if (AssociatedObject == null) return;

        var text = AssociatedObject.Text ?? string.Empty;
        var selectionStart = AssociatedObject.SelectionStart;
        var selectionEnd = AssociatedObject.SelectionEnd;

        // Calculate the new text that would result from this input
        var newText = text;
        if (selectionEnd > selectionStart)
            newText = text.Remove(selectionStart, selectionEnd - selectionStart).Insert(selectionStart, e.Text);
        else
            newText = text.Insert(AssociatedObject.CaretIndex, e.Text);

        // Check if the decimal separator is being added
        if (e.Text == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
            if (!AllowDecimal || text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            {
                e.Handled = true;
                return;
            }

        // Check if the negative sign is being added
        if (e.Text == "-")
            if (!AllowNegative || AssociatedObject.CaretIndex != 0 || text.Contains("-"))
            {
                e.Handled = true;
                return;
            }

        // Handle all other input
        if (!IsValidNumericInput(newText)) e.Handled = true;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (AssociatedObject == null || string.IsNullOrEmpty(AssociatedObject.Text)) return;

        var text = AssociatedObject.Text;
        if (!IsValidNumericInput(text))
        {
            var caretIndex = AssociatedObject.CaretIndex;
            AssociatedObject.Text = FormatValidNumber(text);
            AssociatedObject.CaretIndex = Math.Min(caretIndex, AssociatedObject.Text.Length);
        }
    }

    private bool IsValidNumericInput(string text)
    {
        if (string.IsNullOrEmpty(text)) return true;

        var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        var pattern = AllowDecimal
            ? AllowNegative ? @"^-?\d*\" + decimalSeparator + @"?\d*$" : @"^\d*\" + decimalSeparator + @"?\d*$"
            : AllowNegative
                ? @"^-?\d*$"
                : @"^\d*$";

        return Regex.IsMatch(text, pattern);
    }

    private string FormatValidNumber(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        var cleanedText = new string(text.Where(c => char.IsDigit(c) ||
                                                     (c == '-' && AllowNegative) ||
                                                     (c.ToString() == decimalSeparator && AllowDecimal))
            .ToArray());

        // Ensure only one decimal separator
        if (AllowDecimal && cleanedText.Count(c => c.ToString() == decimalSeparator) > 1)
        {
            var parts = cleanedText.Split(decimalSeparator[0]);
            cleanedText = parts[0] + decimalSeparator + parts[1];
        }

        // Ensure negative sign is only at the start
        if (AllowNegative && cleanedText.Count(c => c == '-') > 0)
        {
            cleanedText = cleanedText.Replace("-", "");
            if (cleanedText.Length > 0) cleanedText = "-" + cleanedText;
        }

        return cleanedText;
    }
}