using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Pinguin.Models;

public partial class PingObject : ObservableObject
{
    [ObservableProperty] private string? _hostName;
    [ObservableProperty] private IPAddress? _ipAddress;
    [ObservableProperty] private int? _pingsSent;
    [ObservableProperty] private int? _pingsReceived;
    [ObservableProperty] private int? _pingsLost;
    [ObservableProperty] private double? _pingPercent;
    [ObservableProperty] private double? _averagePing;
    [ObservableProperty] private ISeries[] _series;
    [ObservableProperty] private bool _graphVisible;
    public ObservableCollection<long?> LineSeries;
    public ObservableCollection<long?> BarSeries;

    public PingObject()
    {
        PingsSent = 0;
        PingsReceived = 0;
        PingsLost = 0;
        GraphVisible = false;
        Series = new ISeries[]
        {
            new LineSeries<long?>
            {
                Values = LineSeries = new ObservableCollection<long?>(),
            },
            new ColumnSeries<long?>
            {
                Values = BarSeries = new ObservableCollection<long?>(),
            }
        };
    }

    public override bool Equals(object? obj)
    {
        var item = obj as PingObject;
        if (item == null) return false;
        return (IpAddress is not null && IpAddress.Equals(item.IpAddress)) || (HostName is not null && HostName.Equals(item.HostName));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IpAddress);
    }


    public void AddReply(PingReply reply)
    {
        if (reply.Status == IPStatus.Success)
        {
            if (AveragePing is null)
                AveragePing = reply.RoundtripTime;
            else
                AveragePing = (AveragePing * PingsReceived + reply.RoundtripTime) / (PingsReceived+1);
            PingsReceived++;
            BarSeries.Add(null);
            if (LineSeries.Count > 0 && reply.RoundtripTime > LineSeries.Max())
            {
                for (int i = 0; i < BarSeries.Count; i++)
                {
                    if (BarSeries[i] is not null)
                        BarSeries[i] = reply.RoundtripTime;
                }
            }
            LineSeries.Add(reply.RoundtripTime);
            
        }
        else
        {
            PingsLost++;
            LineSeries.Add(null);
            BarSeries.Add(LineSeries.Max() ?? 1);
        }
        PingPercent = (double) PingsLost / PingsSent;
    }
}