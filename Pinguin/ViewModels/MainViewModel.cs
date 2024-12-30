using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        var dingus = await this.OpenDialogAsync<PingOptionsViewModel>() as PingOptionsViewModel;
        Console.WriteLine(dingus.Options.HostNames[0]);
        Options = dingus.Options;
        runner.ReplacePings(Options.HostNames.Select(h => new PingObject(h)));
        //Pings.AddRange(Options.HostNames.Select(h => new PingObject(h)));
    }
}