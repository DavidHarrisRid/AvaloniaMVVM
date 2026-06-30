using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.ViewModels;

public partial class StatusBarVm : BaseVm
{
    [ObservableProperty] private string _message = "Ready";
}