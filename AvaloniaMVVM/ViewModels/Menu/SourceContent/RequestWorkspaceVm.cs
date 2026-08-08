using AvaloniaMVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.ViewModels.Menu;

public partial class RequestWorkspaceTabVm : BaseVm
{
    // Jeder offene Tab merkt sich Anfrage und zuletzt gewählte Inhaltsseite.
    public ApiRequestModel Request { get; }

    public bool IsDataDashboardSelected => NavigationPosition == SourceNavigationPosition.DataDashboard;

    public bool IsDashboardDesignerSelected => NavigationPosition == SourceNavigationPosition.DashboardDesigner;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDataDashboardSelected))]
    [NotifyPropertyChangedFor(nameof(IsDashboardDesignerSelected))]
    private SourceNavigationPosition _navigationPosition = SourceNavigationPosition.DataDashboard;

    public RequestWorkspaceTabVm(ApiRequestModel request)
    {
        Request = request;
    }
}
