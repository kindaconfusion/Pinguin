using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class PingOptionsViewModel : ViewModelBase
{
    
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string? _interval = "1";
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string? _packetSize = "32";
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))] private string? _timeout = "4";
    
    
    
    private readonly PingRunner _pingRunner;

    public PingOptionsViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    private bool CanSave()
    {
        return !String.IsNullOrWhiteSpace(Interval) && !String.IsNullOrWhiteSpace(PacketSize) && !String.IsNullOrWhiteSpace(Timeout);
    }
    
    [RelayCommand(CanExecute=nameof(CanSave))]
    public void Save()
    {
        var settings = new Options()
        {
            Interval = Double.Parse(this.Interval),
            PacketSize = Int32.Parse(this.PacketSize),
            Timeout = Double.Parse(this.Timeout)
        };
        _pingRunner.Settings = settings;
    }
}