using System.ComponentModel;
using AvaloniaMVVM.Services;

namespace AvaloniaMVVM.ViewModels;

public class StatusBarVm : BaseVm
{
    // Das ViewModel spiegelt ausschließlich den Zustand des StatusBarService in die View.
    private readonly StatusBarService _statusBarService;

    public string Message
    {
        get => _statusBarService.Message;
        set => _statusBarService.SetMessage(value);
    }

    public StatusBarVm(StatusBarService statusBarService)
    {
        _statusBarService = statusBarService;

        _statusBarService.PropertyChanged += OnStatusBarServicePropertyChanged;
    }

    private void OnStatusBarServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(StatusBarService.Message))
        {
            OnPropertyChanged(nameof(Message));
        }
    }
}
