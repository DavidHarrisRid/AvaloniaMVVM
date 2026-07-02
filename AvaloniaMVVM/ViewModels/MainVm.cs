using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels.Menu;

namespace AvaloniaMVVM.ViewModels;

public partial class MainVm : BaseVm
{
    private readonly StatusBarVm _statusBarVm;
    private readonly MainNavigationService _navigation;
    private readonly DashboardVm _dashboardVm;

    public bool IsDashboardSelected => _navigation.MainNavigationPosition == MainNavigationPosition.Dashboard;
    public bool IsAppSettingsSelected => _navigation.MainNavigationPosition == MainNavigationPosition.AppSettings;
    public bool IsAboutSelected => _navigation.MainNavigationPosition == MainNavigationPosition.About;

    public StatusBarVm StatusBarVm => _statusBarVm;
    public MainNavigationService NavigationService => _navigation;

    public MainVm(
        StatusBarVm statusBar,
        MainNavigationService navigation,
        DashboardVm dashboardVm)
    {
        _statusBarVm = statusBar;
        _navigation = navigation;
        _dashboardVm = dashboardVm;

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

    public void Navigate(MainNavigationPosition position)
    {
        if (position == MainNavigationPosition.Dashboard &&
            _navigation.MainNavigationPosition == MainNavigationPosition.Dashboard)
        {
            _dashboardVm.ToggleSidebar();
            return;
        }

        _navigation.Navigate(position);
    }
}