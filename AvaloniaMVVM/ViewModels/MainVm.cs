using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels.Menu;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels;

public partial class MainVm : BaseVm
{
    
    private readonly StatusBarVm _statusBarVm;
    private readonly MainNavigationService _navigation;
    
    public bool IsDashboardSelected => _navigation.MainNavigationPosition == MainNavigationPosition.Dashboard;
    public bool IsAppSettingsSelected  => _navigation.MainNavigationPosition == MainNavigationPosition.AppSettings;
    public bool IsAboutSelected     => _navigation.MainNavigationPosition == MainNavigationPosition.About;
    
    public StatusBarVm StatusBarVm => _statusBarVm;
    public MainNavigationService NavigationService => _navigation;

    
    public MainVm(StatusBarVm statusBar, MainNavigationService navigation)
    {
        _statusBarVm = statusBar;
        _navigation = navigation;
        
        _navigation.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainNavigationService.MainNavigationPosition))
            {
                OnPropertyChanged(nameof(IsDashboardSelected));
                OnPropertyChanged(nameof(IsAppSettingsSelected));
                OnPropertyChanged(nameof(IsAboutSelected));
            }
        };
    }

    public void Navigate(MainNavigationPosition position) => _navigation.Navigate(position);
}