using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using Pinguin.Models;
using Pinguin.Services;
using Pinguin.Views;
using DialogHost = DialogHostAvalonia.DialogHost;

namespace Pinguin.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private Options _options;
    
    [ObservableProperty] private int _selectedIndex;

    [ObservableProperty] private object _dialog;

    private readonly PingRunner _pingRunner;
    
    public ObservableCollection<PingObject> Pings => _pingRunner.Pings;

    public MainViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    [RelayCommand]
    public async Task AddPing()
    {
        //Dialog = new AddViewModel(_pingRunner);
        var vm = new AddViewModel(_pingRunner);
        var dialog = new TaskDialog
        {
            Content = new ViewLocator().Build(vm),
            DataContext = vm
        };
        //dialog.XamlRoot = TopLevel.GetTopLevel(this);
        await dialog.ShowAsync();
    }
    
    [RelayCommand]
    public async Task OpenPingOptions()
    {
        Dialog = new PingOptionsViewModel(_pingRunner);
        await DialogHost.Show(Dialog);
    }

    [RelayCommand]
    public void DeletePing()
    {
        _pingRunner.RemovePing(Pings[SelectedIndex]);
    }
}