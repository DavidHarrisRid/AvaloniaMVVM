using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels.Menu.SourceContent;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu;

public partial class DashboardVm : BaseVm
{
    private readonly SourceManagementService _sourceManagementService;

    public ObservableCollection<ApiSourceModel> Sources => _sourceManagementService.ApiSources;
    
    public GridLength SidebarColumnWidth => IsSidebarVisible
        ? new GridLength(340)
        : new GridLength(0);

    public ObservableCollection<RequestWorkspaceTabVm> OpenRequestTabs { get; } = new();

    public SourceNavigationService SourceNavigationService { get; }

    public SourceConfiguratorVm SourceConfiguratorVm { get; }

    public RequestConfiguratorVm RequestConfiguratorVm { get; }

    public bool IsEditorVisible => IsSourceEditorOpen || IsRequestEditorOpen;

    public bool IsRequestWorkspaceVisible => !IsEditorVisible;

    public bool IsRequestTabStripVisible => IsRequestWorkspaceVisible && UseRequestTabs;

    public bool IsRequestSingleModeVisible => IsRequestWorkspaceVisible && !UseRequestTabs;

    public bool IsTabsToggleVisible => IsRequestWorkspaceVisible && SelectedRequest is not null;

    public int MainContentColumn => IsSidebarVisible ? 2 : 0;

    public int MainContentColumnSpan => IsSidebarVisible ? 1 : 3;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SidebarColumnWidth))]
    [NotifyPropertyChangedFor(nameof(MainContentColumn))]
    [NotifyPropertyChangedFor(nameof(MainContentColumnSpan))]
    private bool _isSidebarVisible = true;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestWorkspaceVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestTabStripVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestSingleModeVisible))]
    [NotifyPropertyChangedFor(nameof(IsTabsToggleVisible))]
    private bool _isSourceEditorOpen;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestWorkspaceVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestTabStripVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestSingleModeVisible))]
    [NotifyPropertyChangedFor(nameof(IsTabsToggleVisible))]
    private bool _isRequestEditorOpen;

    [ObservableProperty]
    private ApiSourceModel? _selectedSource;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTabsToggleVisible))]
    private ApiRequestModel? _selectedRequest;

    [ObservableProperty]
    private RequestWorkspaceTabVm? _selectedRequestTab;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsRequestTabStripVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestSingleModeVisible))]
    private bool _useRequestTabs = true;

    public DashboardVm(
        SourceManagementService sourceManagementService,
        SourceNavigationService sourceNavigationService,
        SourceConfiguratorVm sourceConfiguratorVm,
        RequestConfiguratorVm requestConfiguratorVm)
    {
        _sourceManagementService = sourceManagementService;
        SourceNavigationService = sourceNavigationService;
        SourceConfiguratorVm = sourceConfiguratorVm;
        RequestConfiguratorVm = requestConfiguratorVm;
    }

    [RelayCommand]
    private void CreateSource()
    {
        SelectedSource = _sourceManagementService.CreateSource();
        SelectedRequest = null;

        SelectedSource.IsExpanded = true;

        OpenSourceEditor();
    }

    [RelayCommand]
    private void ToggleSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = null;

        source.IsExpanded = !source.IsExpanded;
    }

    [RelayCommand]
    private void EditSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = null;

        OpenSourceEditor();
    }

    [RelayCommand]
    private void DeleteSource(ApiSourceModel source)
    {
        var tabsToClose = OpenRequestTabs
            .Where(tab => source.ApiRequests.Contains(tab.Request))
            .ToList();

        foreach (var tab in tabsToClose)
        {
            OpenRequestTabs.Remove(tab);
        }

        var wasSelectedSource = SelectedSource == source;

        _sourceManagementService.DeleteSource(source);

        if (!wasSelectedSource)
        {
            return;
        }

        SelectedSource = null;
        SelectedRequest = null;
        SelectedRequestTab = null;

        if (Sources.Count > 0)
        {
            SelectedSource = Sources[0];
        }

        OpenSourceEditor();
    }

    [RelayCommand]
    private void CreateRequestForSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = _sourceManagementService.CreateRequest(source);

        source.IsExpanded = true;

        OpenRequestEditor();
    }

    [RelayCommand]
    private void OpenRequest(ApiRequestModel request)
    {
        SelectedRequest = request;
        SelectedSource = FindSourceForRequest(request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        if (UseRequestTabs)
        {
            var tab = GetOrCreateTab(request);
            SelectRequestTab(tab);
            return;
        }

        OpenRequestWorkspace(SourceNavigationPosition.DataDashboard);
    }

    [RelayCommand]
    private void EditRequest(ApiRequestModel request)
    {
        SelectedRequest = request;
        SelectedSource = FindSourceForRequest(request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        OpenRequestEditor();
    }

    [RelayCommand]
    private void DeleteRequest(ApiRequestModel request)
    {
        var source = FindSourceForRequest(request);

        if (source is null)
        {
            return;
        }

        var tab = OpenRequestTabs.FirstOrDefault(openTab => openTab.Request == request);

        if (tab is not null)
        {
            OpenRequestTabs.Remove(tab);

            if (SelectedRequestTab == tab)
            {
                SelectedRequestTab = OpenRequestTabs.LastOrDefault();
            }
        }

        var wasSelectedRequest = SelectedRequest == request;

        _sourceManagementService.DeleteRequest(source, request);

        if (wasSelectedRequest)
        {
            SelectedRequest = null;
        }

        if (SelectedRequestTab is not null)
        {
            SelectRequestTab(SelectedRequestTab);
            return;
        }

        if (source.ApiRequests.Count == 0)
        {
            source.IsExpanded = false;
            SelectedSource = source;
            OpenSourceEditor();
        }
    }

    [RelayCommand]
    private void SelectRequestTab(RequestWorkspaceTabVm tab)
    {
        SelectedRequestTab = tab;
        SelectedRequest = tab.Request;
        SelectedSource = FindSourceForRequest(tab.Request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        OpenRequestWorkspace(tab.NavigationPosition);
    }

    [RelayCommand]
    private void CloseRequestTab(RequestWorkspaceTabVm tab)
    {
        var wasSelectedTab = SelectedRequestTab == tab;

        OpenRequestTabs.Remove(tab);

        if (!wasSelectedTab)
        {
            return;
        }

        SelectedRequestTab = OpenRequestTabs.LastOrDefault();

        if (SelectedRequestTab is not null)
        {
            SelectRequestTab(SelectedRequestTab);
            return;
        }

        SelectedRequest = null;
        OpenSourceEditor();
    }

    [RelayCommand]
    private void ToggleRequestTabs()
    {
        UseRequestTabs = !UseRequestTabs;

        if (!UseRequestTabs)
        {
            OpenRequestTabs.Clear();
            SelectedRequestTab = null;

            if (SelectedRequest is not null)
            {
                OpenRequestWorkspace(SourceNavigationPosition.DataDashboard);
            }

            return;
        }

        if (SelectedRequest is not null)
        {
            var tab = GetOrCreateTab(SelectedRequest);
            SelectRequestTab(tab);
        }
    }

    [RelayCommand]
    private void NavigateSourceContent(SourceNavigationPosition position)
    {
        if (UseRequestTabs && SelectedRequestTab is not null)
        {
            SelectedRequestTab.NavigationPosition = position;
        }

        OpenRequestWorkspace(position);
    }

    private RequestWorkspaceTabVm GetOrCreateTab(ApiRequestModel request)
    {
        var existingTab = OpenRequestTabs.FirstOrDefault(tab => tab.Request == request);

        if (existingTab is not null)
        {
            return existingTab;
        }

        var newTab = new RequestWorkspaceTabVm(request);
        OpenRequestTabs.Add(newTab);

        return newTab;
    }

    private ApiSourceModel? FindSourceForRequest(ApiRequestModel request)
    {
        return Sources.FirstOrDefault(source => source.ApiRequests.Contains(request));
    }

    private void OpenSourceEditor()
    {
        SourceConfiguratorVm.Load(SelectedSource);

        IsSourceEditorOpen = true;
        IsRequestEditorOpen = false;
    }

    private void OpenRequestEditor()
    {
        RequestConfiguratorVm.Load(SelectedRequest);

        IsSourceEditorOpen = false;
        IsRequestEditorOpen = true;
    }

    private void OpenRequestWorkspace(SourceNavigationPosition position)
    {
        IsSourceEditorOpen = false;
        IsRequestEditorOpen = false;

        SourceNavigationService.Navigate(position);
    }
    
    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }
}