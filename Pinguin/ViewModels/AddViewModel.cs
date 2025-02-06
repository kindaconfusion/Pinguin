using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class AddViewModel : ViewModelBase
{
    private readonly IPingRunner _pingRunner;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddHostCommand), nameof(AddTraceRouteCommand))]
    private string? _hostName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddHostCommand), nameof(AddTraceRouteCommand), nameof(CloseCommand))]
    private bool _traceRunning;

    public AddViewModel(IPingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    private bool CanAdd()
    {
        return !string.IsNullOrWhiteSpace(HostName) && !TraceRunning;
    }

    private bool CanClose()
    {
        return !TraceRunning;
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