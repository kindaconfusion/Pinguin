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
    
    [ObservableProperty] private double? _interval = 1;
    [ObservableProperty] private int? _packetSize = 32;
    [ObservableProperty] private int? _timeout = 4;
    
    
    
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
            Interval = this.Interval,
            PacketSize = this.PacketSize,
            Timeout = this.Timeout
        };
        _pingRunner.Settings = new Options(Interval.Value, PacketSize.Value);
        DialogHost.Close("Root");
    }
}