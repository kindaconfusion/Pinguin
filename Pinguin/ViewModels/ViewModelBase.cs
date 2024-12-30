using System;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinguin.ViewModels;

public class ViewModelBase : ObservableObject, ICloseable
{
    public event EventHandler? Closed;

    public void RaiseClose()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}