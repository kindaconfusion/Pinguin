using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Pinguin.Models;
using Pinguin.Services;

namespace Pinguin.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private Options _options;
    
    [ObservableProperty] private int _selectedIndex;

    private readonly PingRunner _pingRunner;
    
    public RangeObservableCollection<PingObject> Pings => _pingRunner.Pings;

    public MainViewModel(PingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }
    
    [RelayCommand]
    public async Task OpenPingOptions()
    {
        /*var dingus = await this.OpenDialogAsync<PingOptionsViewModel>() as PingOptionsViewModel;
        */
        var vm = new PingOptionsViewModel(_pingRunner);
        await DialogHost.Show(vm);
    }

    [RelayCommand]
    public void DeletePing()
    {
        _pingRunner.RemovePing(Pings[_selectedIndex]);
    }
}