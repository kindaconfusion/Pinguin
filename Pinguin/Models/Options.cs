using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinguin.Models;

public partial class Options : ObservableObject
{
    [ObservableProperty] private double? _interval;
    [ObservableProperty] private int? _packetSize;

    public Options(double interval, int packetSize)
    {
        _interval = interval;
        _packetSize = packetSize;
    }
}