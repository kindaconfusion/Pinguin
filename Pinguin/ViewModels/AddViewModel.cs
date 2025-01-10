using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class AddViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddHostCommand), nameof(AddTraceRouteCommand))] private string? _hostName;
    private readonly PingRunner _pingRunner;
    
    private bool CanAdd() { return !string.IsNullOrWhiteSpace(HostName); }
    
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
        await _pingRunner.Tracert(HostName);
        HostName = string.Empty;
        try
        {
            //DialogHost.Close("Root");
        }
        catch (InvalidOperationException ex)
        {
            // the user closed the dialog before the traceroute completed.
            // this is acceptable
            // (there may be a cleaner way to do this but i think it's fine this way)
        }
    }
}