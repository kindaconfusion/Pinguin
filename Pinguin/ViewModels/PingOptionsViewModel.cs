using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class PingOptionsViewModel : ViewModelBase
{
    [ObservableProperty] private string? _hostNames;
    [ObservableProperty] private double? _interval = 1;
    [ObservableProperty] private int? _packetSize = 32;
    
    [ObservableProperty] private Options _options;
    
    [RelayCommand]
    public void Save()
    {
        Options = new Options(HostNames.Split('\n')
            .Select(h => h.Trim())
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .ToList(), Interval.Value, PacketSize.Value);
        RaiseClose();
    }
}