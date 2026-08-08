using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;

namespace AvaloniaMVVM.ViewModels;

public partial class MainVm : BaseVm
{
    // Das Root-ViewModel verbindet Hauptnavigation, Dashboard und Statusleiste.
    private readonly StatusBarVm _statusBarVm;
    private readonly MainNavigationService _navigation;
    private readonly DashboardWorkspaceService _dashboardWorkspaceService;

    public bool IsDashboardSelected => _navigation.MainNavigationPosition == MainNavigationPosition.Dashboard;
    public bool IsAppSettingsSelected => _navigation.MainNavigationPosition == MainNavigationPosition.AppSettings;
    public bool IsAboutSelected => _navigation.MainNavigationPosition == MainNavigationPosition.About;

    public StatusBarVm StatusBarVm => _statusBarVm;
    public MainNavigationService NavigationService => _navigation;

    public MainVm(
        StatusBarVm statusBar,
        MainNavigationService navigation,
        DashboardWorkspaceService dashboardWorkspaceService)
    {
        _statusBarVm = statusBar;
        _navigation = navigation;
        _dashboardWorkspaceService = dashboardWorkspaceService;

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
        // Ein erneuter Klick auf Home blendet die Dashboard-Seitenleiste um.
        if (position == MainNavigationPosition.Dashboard &&
            _navigation.MainNavigationPosition == MainNavigationPosition.Dashboard)
        {
            _dashboardWorkspaceService.ToggleSidebar();
            return;
        }

        _navigation.Navigate(position);
    }
}
