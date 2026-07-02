using AvaloniaMVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.ViewModels.Menu;

public partial class RequestWorkspaceTabVm : BaseVm
{
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