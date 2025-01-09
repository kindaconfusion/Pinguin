using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class PingOptionsViewModel : ViewModelBase
{
    [ObservableProperty] private string? _hostName;
    [ObservableProperty] private double? _interval = 1;
    [ObservableProperty] private int? _packetSize = 32;
    
    [ObservableProperty] private Options _options;
    
    [ObservableProperty] private PingObject _newPing;
    
    //private readonly TaskCompletionSource<Options> _dialogResult;
    
    private readonly PingRunner _pingRunner;

    public PingOptionsViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
        //_dialogResult = new TaskCompletionSource<Options>();
    }
    
    //public Task<Options> GetResultAsync() => _dialogResult.Task;

    [RelayCommand]
    public void AddHost()
    {
        _pingRunner.AddPing(HostName);
        HostName = string.Empty;
    }

    [RelayCommand]
    public async Task AddTraceRoute()
    {
        await _pingRunner.Tracert(HostName);
        HostName = string.Empty;
        try
        {
            DialogHost.Close("Root", Options);
        }
        catch (InvalidOperationException ex)
        {
            // the user closed the dialog before the traceroute completed.
            // this is acceptable
            // (there may be a cleaner way to do this but i think it's fine this way)
        }
    }
    
    [RelayCommand]
    public void Save()
    {
        
        //_dialogResult.SetResult(Options);
        DialogHost.Close("Root", Options);
        //RaiseClose();
    }
}