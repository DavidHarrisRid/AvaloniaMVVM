using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels.Menu;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Services;

public partial class DashboardWorkspaceService : ObservableObject
{
    // Dieser Dienst buendelt Auswahl, Editorzustand und geoeffnete Request-Tabs.
    private readonly SourceManagementService _sourceManagementService;
    private readonly SourceNavigationService _sourceNavigationService;
    private readonly TabReorderService _tabReorderService;

    public ObservableCollection<RequestWorkspaceTabVm> OpenRequestTabs { get; } = new();

    public bool IsEditorVisible => IsSourceEditorOpen || IsRequestEditorOpen;

    public bool IsRequestWorkspaceVisible => !IsEditorVisible;

    public bool IsRequestTabStripVisible => IsRequestWorkspaceVisible && UseRequestTabs;

    public bool IsRequestSingleModeVisible => IsRequestWorkspaceVisible && !UseRequestTabs;

    public bool IsTabsToggleVisible => IsRequestWorkspaceVisible && SelectedRequest is not null;

    public GridLength SidebarColumnWidth => IsSidebarVisible
        ? new GridLength(340)
        : new GridLength(0);

    // Spalte und Span verschieben den Hauptinhalt passend zur ein- oder ausgeblendeten Seitenleiste.
    public int MainContentColumn => IsSidebarVisible ? 2 : 0;

    public int MainContentColumnSpan => IsSidebarVisible ? 1 : 3;

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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SidebarColumnWidth))]
    [NotifyPropertyChangedFor(nameof(MainContentColumn))]
    [NotifyPropertyChangedFor(nameof(MainContentColumnSpan))]
    private bool _isSidebarVisible = true;

    public DashboardWorkspaceService(
        SourceManagementService sourceManagementService,
        SourceNavigationService sourceNavigationService,
        TabReorderService tabReorderService)
    {
        _sourceManagementService = sourceManagementService;
        _sourceNavigationService = sourceNavigationService;
        _tabReorderService = tabReorderService;
    }

    public ApiSourceModel CreateSource()
    {
        // Eine neue Quelle wird sofort ausgewaehlt, aufgeklappt und im Editor geoeffnet.
        SelectedSource = _sourceManagementService.CreateSource();
        SelectedRequest = null;

        SelectedSource.IsExpanded = true;

        OpenSourceEditor();

        return SelectedSource;
    }

    public void ToggleSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = null;

        source.IsExpanded = !source.IsExpanded;
    }

    public void EditSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = null;

        OpenSourceEditor();
    }

    public void DeleteSource(ApiSourceModel source)
    {
        // Vor dem Loeschen der Quelle werden alle zugehoerigen Tabs aus dem Arbeitsbereich entfernt.
        var tabsToClose = OpenRequestTabs
            .Where(tab => source.ApiRequests.Contains(tab.Request))
            .ToList();

        foreach (var tab in tabsToClose)
        {
            OpenRequestTabs.Remove(tab);
        }

        if (SelectedRequestTab is not null &&
            source.ApiRequests.Contains(SelectedRequestTab.Request))
        {
            SelectedRequestTab = null;
        }

        var wasSelectedSource = SelectedSource == source;

        _sourceManagementService.DeleteSource(source);

        if (!wasSelectedSource)
        {
            return;
        }

        SelectedSource = null;
        SelectedRequest = null;

        if (_sourceManagementService.ApiSources.Count > 0)
        {
            SelectedSource = _sourceManagementService.ApiSources[0];
        }

        OpenSourceEditor();
    }

    public ApiRequestModel CreateRequestForSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = _sourceManagementService.CreateRequest(source);

        source.IsExpanded = true;

        OpenRequestEditor();

        return SelectedRequest;
    }

    public void OpenRequest(ApiRequestModel request)
    {
        SelectedRequest = request;
        SelectedSource = _sourceManagementService.FindSourceForRequest(request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        // Im Tab-Modus wird ein bestehender Tab aktiviert oder einmalig angelegt.
        if (UseRequestTabs)
        {
            var tab = GetOrCreateTab(request);
            SelectRequestTab(tab);
            return;
        }

        OpenRequestWorkspace(SourceNavigationPosition.DataDashboard);
    }

    public void EditRequest(ApiRequestModel request)
    {
        SelectedRequest = request;
        SelectedSource = _sourceManagementService.FindSourceForRequest(request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        OpenRequestEditor();
    }

    public void DeleteRequest(ApiRequestModel request)
    {
        var source = _sourceManagementService.FindSourceForRequest(request);

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

    public void SelectRequestTab(RequestWorkspaceTabVm tab)
    {
        // Die Tab-Auswahl stellt zugleich Anfrage, Quelle und zuletzt besuchte Inhaltsseite wieder her.
        SelectedRequestTab = tab;
        SelectedRequest = tab.Request;
        SelectedSource = _sourceManagementService.FindSourceForRequest(tab.Request);

        if (SelectedSource is not null)
        {
            SelectedSource.IsExpanded = true;
        }

        OpenRequestWorkspace(tab.NavigationPosition);
    }

    public void CloseRequestTab(RequestWorkspaceTabVm tab)
    {
        var wasSelectedTab = SelectedRequestTab == tab;

        OpenRequestTabs.Remove(tab);

        if (!wasSelectedTab)
        {
            return;
        }

        SelectedRequestTab = OpenRequestTabs.LastOrDefault();

        // Nach dem Schliessen bleibt der zuletzt geoeffnete Tab als naechster Arbeitskontext aktiv.
        if (SelectedRequestTab is not null)
        {
            SelectRequestTab(SelectedRequestTab);
            return;
        }

        SelectedRequest = null;
        OpenSourceEditor();
    }
    
    public void ReorderRequestTab(TabReorderRequest request)
    {
        _tabReorderService.Reorder(OpenRequestTabs, request);
    }

    public void ToggleRequestTabs()
    {
        UseRequestTabs = !UseRequestTabs;

        // Beim Abschalten werden alle offenen Tabs wie vom Benutzer erwartet verworfen.
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

    public void NavigateSourceContent(SourceNavigationPosition position)
    {
        // Jeder Tab behaelt seine Inhaltsseite, damit sie beim Wechsel wiederhergestellt werden kann.
        if (UseRequestTabs && SelectedRequestTab is not null)
        {
            SelectedRequestTab.NavigationPosition = position;
        }

        OpenRequestWorkspace(position);
    }

    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }

    public void OpenSourceEditor()
    {
        IsSourceEditorOpen = true;
        IsRequestEditorOpen = false;
    }

    public void OpenRequestEditor()
    {
        IsSourceEditorOpen = false;
        IsRequestEditorOpen = true;
    }

    private void OpenRequestWorkspace(SourceNavigationPosition position)
    {
        // Der Arbeitsbereich ist nur sichtbar, wenn keiner der beiden Konfiguratoren geoeffnet ist.
        IsSourceEditorOpen = false;
        IsRequestEditorOpen = false;

        _sourceNavigationService.Navigate(position);
    }

    private RequestWorkspaceTabVm GetOrCreateTab(ApiRequestModel request)
    {
        // Pro Anfrage darf hoechstens ein Tab existieren, damit kein doppelter Arbeitszustand entsteht.
        var existingTab = OpenRequestTabs.FirstOrDefault(tab => tab.Request == request);

        if (existingTab is not null)
        {
            return existingTab;
        }

        var newTab = new RequestWorkspaceTabVm(request);
        OpenRequestTabs.Add(newTab);

        return newTab;
    }
}
