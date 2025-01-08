using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class PingOptionsViewModel : ViewModelBase
{
    [ObservableProperty] private string? _hostNames;
    [ObservableProperty] private double? _interval = 1;
    [ObservableProperty] private int? _packetSize = 32;
    
    [ObservableProperty] private Options _options;
    
    private readonly TaskCompletionSource<Options> _dialogResult;

    public PingOptionsViewModel()
    {
        _dialogResult = new TaskCompletionSource<Options>();
    }
    
    public Task<Options> GetResultAsync() => _dialogResult.Task;
    
    [RelayCommand]
    public void Save()
    {
        Options = new Options(HostNames.Split('\n')
            .Select(h => h.Trim())
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .ToList(), Interval.Value, PacketSize.Value);
        //_dialogResult.SetResult(Options);
        DialogHost.Close("Root", Options);
        //RaiseClose();
    }
}