using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinguin.Models;

public partial class Options : ObservableObject
{
    [ObservableProperty] private List<string>? _hostNames;
    [ObservableProperty] private double? _interval;
    [ObservableProperty] private int? _packetSize;

    public Options(List<String> hosts, double interval, int packetSize)
    {
        _hostNames = hosts;
        _interval = interval;
        _packetSize = packetSize;
    }
}