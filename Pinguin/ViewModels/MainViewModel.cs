using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Pinguin.Services;

namespace Pinguin.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [RelayCommand]
    public async Task OpenPingOptions()
    {
        await this.OpenDialogAsync<PingOptionsViewModel>();
    }
}