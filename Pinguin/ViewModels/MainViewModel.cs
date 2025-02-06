﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using LiveChartsCore;
using Pinguin.Models;
using Pinguin.Services;
using Pinguin.Views;

namespace Pinguin.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private Options _options;
    
    [ObservableProperty] private int _selectedIndex;

    [ObservableProperty] private object _dialog;

    [ObservableProperty] private ObservableCollection<ISeries[]> _series = new();

    [ObservableProperty] private bool _isUpdateAvailable;

    private readonly PingRunner _pingRunner;
    
    public ObservableCollection<PingObject> Pings => _pingRunner.Pings;
    

    public MainViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
        IsUpdateAvailable = false;
        CheckForUpdates();
    }

    private async void CheckForUpdates()
    {
        IsUpdateAvailable = await UpdateChecker.CheckForUpdates();
    }
    
    [RelayCommand]
    public void OpenGraph(PingObject ping)
    {
        Series.Add(ping.Series);
        ping.GraphVisible = true;
        //Series = ping.Series;
    }

    [RelayCommand]
    public void DeletePing()
    {
        if (Pings.ElementAtOrDefault(SelectedIndex) != null) _pingRunner.RemovePing(Pings[SelectedIndex]);
    }
}