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

    private PingRunner runner = new PingRunner(null);
    
    public RangeObservableCollection<PingObject> Pings => runner.Pings;
    
    
    [RelayCommand]
    public async Task OpenPingOptions()
    {
        /*var dingus = await this.OpenDialogAsync<PingOptionsViewModel>() as PingOptionsViewModel;
        */
        
        var dicks = await DialogHost.Show(new PingOptionsViewModel()) as Options;
        if (dicks != null) runner.ReplacePings(dicks.HostNames.Select(h => new PingObject(h)));
    }
}