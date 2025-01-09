using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinguin.Models;

public partial class Options : ObservableObject
{
    [ObservableProperty] private double? _interval;
    [ObservableProperty] private int? _packetSize;
    [ObservableProperty] private int? _timeout;

    public Options()
    {
        _interval = 1;
        _packetSize = 32;
        _timeout = 4;
    }
    public Options(double interval, int packetSize)
    {
        _interval = interval;
        _packetSize = packetSize;
    }
}