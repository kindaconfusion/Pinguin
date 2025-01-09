using System;
using System.Net;
using System.Net.NetworkInformation;
using CommunityToolkit.Mvvm.ComponentModel;

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
    
    public PingObject(string hostName)
    {
        _hostName = hostName;
        PingsReceived = 0;
        PingsSent = 0;
        PingsLost = 0;
        PingPercent = 0.0;
    }
    
    public PingObject(IPAddress ipAddress)
    {
        _ipAddress = ipAddress;
        PingsReceived = 0;
        PingsSent = 0;
        PingsLost = 0;
        PingPercent = 0.0;
    }


    public void AddReply(PingReply reply)
    {
        if (reply.Status == IPStatus.Success)
        {
            if (this.AveragePing is null)
                this.AveragePing = reply.RoundtripTime;
            else
                this.AveragePing = ((this.AveragePing * this.PingsReceived) + reply.RoundtripTime) / (this.PingsReceived+1);
            this.PingsReceived++;
            Console.WriteLine(this.AveragePing);
        }
        else
        {
            this.PingsLost++;
        }
        this.PingPercent = 1 - ((double) this.PingsReceived / this.PingsSent);
    }
}