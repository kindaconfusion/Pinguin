using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Pinguin.ViewModels;

namespace Pinguin.Services;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        string name;
        if (param is Type)
        {
            name = (param as Type).FullName.Replace("ViewModel", "View", StringComparison.Ordinal);
            ;
        }
        else
        {
            name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        }

        var type = Type.GetType(name);

        if (type != null) return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}