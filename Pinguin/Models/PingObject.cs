using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pinguin.Models;

public partial class PingObject : ObservableObject
{
    [ObservableProperty] private string _hostName;
    [ObservableProperty] private IPAddress? _ipAddress;
    [ObservableProperty] private int? _pingsSent;
    [ObservableProperty] private int? _pingsReceived;
    [ObservableProperty] private int? _pingsLost;
    [ObservableProperty] private double? _pingPercent;
    [ObservableProperty] private double? _averagePing;
    
    public PingObject(string hostName)
    {
        _hostName = hostName;

    }
}