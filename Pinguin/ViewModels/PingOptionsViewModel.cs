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
    
    
    
    private readonly PingRunner _pingRunner;

    public PingOptionsViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    
    
    [RelayCommand]
    public void Save()
    {
        //_dialogResult.SetResult(Options);
        DialogHost.Close("Root", Options);
        //RaiseClose();
    }
}