using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPingRunner _pingRunner;

    [ObservableProperty] private object _dialog;

    [ObservableProperty] private bool _isUpdateAvailable;
    [ObservableProperty] private Options _options;

    [ObservableProperty] private int _selectedIndex;

    [ObservableProperty] private ObservableCollection<ISeries[]> _series = new();


    public MainViewModel(IPingRunner pingRunner)
    {
        _pingRunner = pingRunner;
        IsUpdateAvailable = false;
        CheckForUpdates();
    }

    public ObservableCollection<PingObject> Pings => _pingRunner.Pings;

    private async void CheckForUpdates()
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version.ToString();
        version = version.Substring(0, version.Length - 2);
        IsUpdateAvailable = await UpdateChecker.CheckForUpdates(version);
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