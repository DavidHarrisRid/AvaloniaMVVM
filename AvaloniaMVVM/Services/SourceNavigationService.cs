using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class SourceNavigationService : ObservableObject
{
    [ObservableProperty]
    private object _currentViewModel;

    [ObservableProperty]
    private SourceNavigationPosition _sourceNavigationPosition;

    private readonly RequestConfiguratorVm _requestConfiguratorVm;
    private readonly DataDashboardVm _dataDashboardVm;
    private readonly DashboardDesignerVm _dashboardDesignerVm;

    public SourceNavigationService(
        RequestConfiguratorVm requestConfiguratorVm,
        DataDashboardVm dataDashboardVm,
        DashboardDesignerVm dashboardDesignerVm)
    {
        _requestConfiguratorVm = requestConfiguratorVm;
        _dataDashboardVm = dataDashboardVm;
        _dashboardDesignerVm = dashboardDesignerVm;

        Navigate(SourceNavigationPosition.RequestConfigurator);
    }

    public void Navigate(SourceNavigationPosition position)
    {
        switch (position)
        {
            case SourceNavigationPosition.RequestConfigurator:
                CurrentViewModel = _requestConfiguratorVm;
                break;

            case SourceNavigationPosition.DataDashboard:
                CurrentViewModel = _dataDashboardVm;
                break;

            case SourceNavigationPosition.DashboardDesigner:
                CurrentViewModel = _dashboardDesignerVm;
                break;
        }

        SourceNavigationPosition = position;
    }
}