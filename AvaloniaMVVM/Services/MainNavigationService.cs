using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels.Menu;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class MainNavigationService : ObservableObject
{
    // CurrentViewModel ist der bindbare Inhalt des zentralen Seitenbereichs.
    [ObservableProperty] private object _currentViewModel;
    
    [ObservableProperty] private MainNavigationPosition _mainNavigationPosition;
    
    private readonly AboutVm _aboutVm;
    private readonly AppSettingsVm _appSettingsVm;
    private readonly DashboardVm _dashboardVm; 
    
    public MainNavigationService(AboutVm  aboutVm, AppSettingsVm appSettingsVm, DashboardVm dashboardVm)
    {
        _aboutVm = aboutVm;
        _appSettingsVm = appSettingsVm;
        _dashboardVm = dashboardVm;
        
        Navigate(MainNavigationPosition.Dashboard);
    }

    public void Navigate(MainNavigationPosition position)
    {
        // Die Enum-Position wird auf genau ein registriertes Seiten-ViewModel abgebildet.
        switch (position)
        {
            case MainNavigationPosition.About:
                CurrentViewModel = _aboutVm;
                break;
            case MainNavigationPosition.Dashboard:  
                CurrentViewModel = _dashboardVm;
                break;
            case MainNavigationPosition.AppSettings:
                CurrentViewModel = _appSettingsVm;
                break;
        }
        MainNavigationPosition = position;
    }


}
