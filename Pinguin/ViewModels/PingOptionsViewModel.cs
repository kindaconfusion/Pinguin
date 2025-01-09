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
    
    [ObservableProperty] private string? _interval = "1";
    [ObservableProperty] private string? _packetSize = "32";
    [ObservableProperty] private string? _timeout = "4";
    
    
    
    private readonly PingRunner _pingRunner;

    public PingOptionsViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    
    
    [RelayCommand]
    public void Save()
    {
        var settings = new Options()
        {
            Interval = Double.Parse(this.Interval), // todo prevent null states
            PacketSize = Int32.Parse(this.PacketSize),
            Timeout = Double.Parse(this.Timeout)
        };
        _pingRunner.Settings = settings;
        DialogHost.Close("Root");
    }
}