using System;
using FluentAvalonia.UI.Controls;
using Pinguin.Services;
using Pinguin.Styles;
using Pinguin.ViewModels;

namespace Pinguin.Views;

public partial class AddView : ContentDialog
{
    public AddView()
    {
        InitializeComponent();
        DataContext = ServiceLocator.Instance.GetService(typeof(AddViewModel));
        if (DataContext is AddViewModel vm)
        {
            vm.Closed += Hide;
            // this should really be built in, this is so fucking stupid
            vm.AddHostCommand.CanExecuteChanged +=
                (_, __) => IsPrimaryButtonEnabled = vm.AddHostCommand.CanExecute(null);
            vm.AddTraceRouteCommand.CanExecuteChanged += (_, __) =>
                IsSecondaryButtonEnabled = vm.AddTraceRouteCommand.CanExecute(null);
            vm.CloseCommand.CanExecuteChanged += (_, __) =>
                ContentDialogExtensions.SetIsCloseButtonEnabled(this, vm.CloseCommand.CanExecute(null));
        }

        IsPrimaryButtonEnabled = false;
        IsSecondaryButtonEnabled = false;
    }

    protected override Type StyleKeyOverride => typeof(ContentDialog);

    private void Hide(object? sender, EventArgs e)
    {
        Hide();
    }

    private void ContentDialog_OnClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
    {
        if (DataContext is AddViewModel addViewModel && addViewModel.TraceRunning) args.Cancel = true;
    }
}