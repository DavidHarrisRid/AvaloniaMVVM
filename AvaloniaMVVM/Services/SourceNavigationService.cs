using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class SourceNavigationService : ObservableObject
{
    // Diese Navigation ist vom Hauptmenü getrennt und gilt nur für den Request-Arbeitsbereich.
    [ObservableProperty]
    private object _currentViewModel;

    [ObservableProperty]
    private SourceNavigationPosition _sourceNavigationPosition;

    private readonly DataDashboardVm _dataDashboardVm;
    private readonly DashboardDesignerVm _dashboardDesignerVm;

    public SourceNavigationService(
        DataDashboardVm dataDashboardVm,
        DashboardDesignerVm dashboardDesignerVm)
    {
        _dataDashboardVm = dataDashboardVm;
        _dashboardDesignerVm = dashboardDesignerVm;

        Navigate(SourceNavigationPosition.DataDashboard);
    }

    public void Navigate(SourceNavigationPosition position)
    {
        // Die Request-Unterseite wird unabhängig von der globalen Navigation gewechselt.
        switch (position)
        {
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
