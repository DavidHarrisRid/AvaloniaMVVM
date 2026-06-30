using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels.Menu;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class SourceNavigationService : ObservableObject
{
    [ObservableProperty] private object _currentViewModel;
    
    [ObservableProperty] private SourceNavigationPosition _sourceNavigationPosition;
    
    
    private readonly SourceConfiguratorVm _sourceConfiguratorVm;
    private readonly DataDashboardVm _dataDashboardVm;
    private readonly DashboardDesignerVm _dashboardDesignerVm;

    public SourceNavigationService(
        SourceConfiguratorVm sourceConfiguratorVm,
        DataDashboardVm dataDashboardVm,
        DashboardDesignerVm dashboardDesignerVm)
    {
        _sourceConfiguratorVm = sourceConfiguratorVm;
        _dataDashboardVm = dataDashboardVm;
        _dashboardDesignerVm = dashboardDesignerVm;

        Navigate(SourceNavigationPosition.Configurator);
    }
    
    
    public void Navigate(SourceNavigationPosition position)
    {
        switch (position)
        {
            case SourceNavigationPosition.Configurator:
                CurrentViewModel = _sourceConfiguratorVm;
                break;
            case SourceNavigationPosition.Dashboard:  
                CurrentViewModel = _dataDashboardVm;
                break;
            case SourceNavigationPosition.Designer:
                CurrentViewModel = _dashboardDesignerVm;
                break;
        }
        SourceNavigationPosition = position;
    }
}