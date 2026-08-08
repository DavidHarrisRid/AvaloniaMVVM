using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;
using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu;

public partial class DashboardVm : BaseVm
{
    // Das ViewModel reicht UI-Aktionen weiter; die eigentliche Logik bleibt in den Diensten.
    private readonly SourceManagementService _sourceManagementService;
    private readonly DashboardWorkspaceService _workspace;
    private readonly DeleteConfirmationService _deleteConfirmationService;

    public ObservableCollection<ApiSourceModel> Sources => _sourceManagementService.ApiSources;

    public ObservableCollection<RequestWorkspaceTabVm> OpenRequestTabs => _workspace.OpenRequestTabs;

    public SourceNavigationService SourceNavigationService { get; }

    public SourceConfiguratorVm SourceConfiguratorVm { get; }

    public RequestConfiguratorVm RequestConfiguratorVm { get; }

    public DeleteConfirmationService DeleteConfirmationService => _deleteConfirmationService;

    public bool IsEditorVisible => _workspace.IsEditorVisible;
    public bool IsRequestWorkspaceVisible => _workspace.IsRequestWorkspaceVisible;
    public bool IsRequestTabStripVisible => _workspace.IsRequestTabStripVisible;
    public bool IsRequestSingleModeVisible => _workspace.IsRequestSingleModeVisible;
    public bool IsTabsToggleVisible => _workspace.IsTabsToggleVisible;

    public bool IsSourceEditorOpen => _workspace.IsSourceEditorOpen;
    public bool IsRequestEditorOpen => _workspace.IsRequestEditorOpen;
    public bool IsSidebarVisible => _workspace.IsSidebarVisible;

    public GridLength SidebarColumnWidth => _workspace.SidebarColumnWidth;
    public int MainContentColumn => _workspace.MainContentColumn;
    public int MainContentColumnSpan => _workspace.MainContentColumnSpan;

    public ApiSourceModel? SelectedSource
    {
        get => _workspace.SelectedSource;
        set => _workspace.SelectedSource = value;
    }

    public ApiRequestModel? SelectedRequest
    {
        get => _workspace.SelectedRequest;
        set => _workspace.SelectedRequest = value;
    }

    public RequestWorkspaceTabVm? SelectedRequestTab
    {
        get => _workspace.SelectedRequestTab;
        set => _workspace.SelectedRequestTab = value;
    }

    public bool UseRequestTabs
    {
        get => _workspace.UseRequestTabs;
        set => _workspace.UseRequestTabs = value;
    }

    public DashboardVm(
        SourceManagementService sourceManagementService,
        DashboardWorkspaceService workspace,
        DeleteConfirmationService deleteConfirmationService,
        SourceNavigationService sourceNavigationService,
        SourceConfiguratorVm sourceConfiguratorVm,
        RequestConfiguratorVm requestConfiguratorVm)
    {
        _sourceManagementService = sourceManagementService;
        _workspace = workspace;
        _deleteConfirmationService = deleteConfirmationService;

        SourceNavigationService = sourceNavigationService;
        SourceConfiguratorVm = sourceConfiguratorVm;
        RequestConfiguratorVm = requestConfiguratorVm;

        _workspace.PropertyChanged += OnWorkspacePropertyChanged;
        _deleteConfirmationService.PropertyChanged += OnDeleteConfirmationPropertyChanged;
    }

    [RelayCommand]
    private void CreateSource()
    {
        _workspace.CreateSource();
        SourceConfiguratorVm.Load(_workspace.SelectedSource);
        RefreshAll();
    }

    [RelayCommand]
    private void ToggleSource(ApiSourceModel source)
    {
        _workspace.ToggleSource(source);
        RefreshAll();
    }

    [RelayCommand]
    private void EditSource(ApiSourceModel source)
    {
        _workspace.EditSource(source);
        SourceConfiguratorVm.Load(_workspace.SelectedSource);
        RefreshAll();
    }

    [RelayCommand]
    private void DeleteSource(ApiSourceModel source)
    {
        _deleteConfirmationService.OpenForSource(source);
    }

    [RelayCommand]
    private void CreateRequestForSource(ApiSourceModel source)
    {
        _workspace.CreateRequestForSource(source);
        RequestConfiguratorVm.Load(_workspace.SelectedRequest);
        RefreshAll();
    }

    [RelayCommand]
    private void OpenRequest(ApiRequestModel request)
    {
        _workspace.OpenRequest(request);
        RefreshAll();
    }

    [RelayCommand]
    private void EditRequest(ApiRequestModel request)
    {
        _workspace.EditRequest(request);
        RequestConfiguratorVm.Load(_workspace.SelectedRequest);
        RefreshAll();
    }

    [RelayCommand]
    private void DeleteRequest(ApiRequestModel request)
    {
        _deleteConfirmationService.OpenForRequest(request);
    }

    [RelayCommand]
    private void ConfirmDelete()
    {
        _deleteConfirmationService.Confirm();

        SourceConfiguratorVm.Load(_workspace.SelectedSource);
        RequestConfiguratorVm.Load(_workspace.SelectedRequest);

        RefreshAll();
    }

    [RelayCommand]
    private void CancelDelete()
    {
        _deleteConfirmationService.Cancel();
    }

    [RelayCommand]
    private void SelectRequestTab(RequestWorkspaceTabVm tab)
    {
        _workspace.SelectRequestTab(tab);
        RefreshAll();
    }

    [RelayCommand]
    private void CloseRequestTab(RequestWorkspaceTabVm tab)
    {
        _workspace.CloseRequestTab(tab);
        SourceConfiguratorVm.Load(_workspace.SelectedSource);
        RefreshAll();
    }
    
    [RelayCommand]
    private void ReorderRequestTab(TabReorderRequest request)
    {
        _workspace.ReorderRequestTab(request);
        RefreshAll();
    }

    [RelayCommand]
    private void ToggleRequestTabs()
    {
        _workspace.ToggleRequestTabs();
        RefreshAll();
    }

    [RelayCommand]
    private void NavigateSourceContent(SourceNavigationPosition position)
    {
        _workspace.NavigateSourceContent(position);
        RefreshAll();
    }

    public void ToggleSidebar()
    {
        _workspace.ToggleSidebar();
        RefreshAll();
    }

    private void OnWorkspacePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.PropertyName))
        {
            OnPropertyChanged(e.PropertyName);
        }

        RefreshComputedProperties();
    }

    private void OnDeleteConfirmationPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(DeleteConfirmationService));
    }

    private void RefreshAll()
    {
        OnPropertyChanged(nameof(Sources));
        OnPropertyChanged(nameof(OpenRequestTabs));
        OnPropertyChanged(nameof(SelectedSource));
        OnPropertyChanged(nameof(SelectedRequest));
        OnPropertyChanged(nameof(SelectedRequestTab));
        OnPropertyChanged(nameof(UseRequestTabs));
        OnPropertyChanged(nameof(DeleteConfirmationService));

        RefreshComputedProperties();
    }

    private void RefreshComputedProperties()
    {
        // Abgeleitete Bindings werden nach einer Zustandsaenderung gemeinsam aktualisiert.
        OnPropertyChanged(nameof(IsEditorVisible));
        OnPropertyChanged(nameof(IsRequestWorkspaceVisible));
        OnPropertyChanged(nameof(IsRequestTabStripVisible));
        OnPropertyChanged(nameof(IsRequestSingleModeVisible));
        OnPropertyChanged(nameof(IsTabsToggleVisible));
        OnPropertyChanged(nameof(IsSourceEditorOpen));
        OnPropertyChanged(nameof(IsRequestEditorOpen));
        OnPropertyChanged(nameof(IsSidebarVisible));
        OnPropertyChanged(nameof(SidebarColumnWidth));
        OnPropertyChanged(nameof(MainContentColumn));
        OnPropertyChanged(nameof(MainContentColumnSpan));
    }
}
