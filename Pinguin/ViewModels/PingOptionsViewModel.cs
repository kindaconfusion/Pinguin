using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Models;

namespace Pinguin.ViewModels;

public partial class PingOptionsViewModel : ViewModelBase
{
    private readonly IPingRunner _pingRunner;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string? _interval = "1";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string? _packetSize = "32";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string? _timeout = "4";

    public PingOptionsViewModel(IPingRunner pingRunner)
    {
        _pingRunner = pingRunner;
    }

    public bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Interval) && !string.IsNullOrWhiteSpace(PacketSize) &&
               !string.IsNullOrWhiteSpace(Timeout);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public void Save()
    {
        var settings = new Options
        {
            Interval = double.Parse(Interval),
            PacketSize = int.Parse(PacketSize),
            Timeout = int.Parse(Timeout)
        };
        _pingRunner.Settings = settings;
    }
}