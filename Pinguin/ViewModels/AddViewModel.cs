using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class AddViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddHostCommand), nameof(AddTraceRouteCommand))] private string? _hostName;
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddHostCommand), nameof(AddTraceRouteCommand), nameof(CloseCommand))] private bool _traceRunning;
    private readonly PingRunner _pingRunner;
    
    private bool CanAdd() { return !string.IsNullOrWhiteSpace(HostName) && !TraceRunning; }
    private bool CanClose() => !TraceRunning;
    
    public AddViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }
    
    [RelayCommand(CanExecute = nameof(CanAdd))]
    public void AddHost()
    {
        _pingRunner.AddPing(HostName);
        HostName = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(CanAdd))]
    public async Task AddTraceRoute()
    {
        TraceRunning = true;
        await _pingRunner.Tracert(HostName);
        HostName = string.Empty;
        TraceRunning = false;
        RaiseClose();
    }

    [RelayCommand(CanExecute = nameof(CanClose))]
    private void Close()
    {
        RaiseClose();
    }
}