using System.Collections.ObjectModel;
using System.Linq;
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

    public SourceNavigationService SourceNavigationService { get; }

    public SourceConfiguratorVm SourceConfiguratorVm { get; }

    public RequestConfiguratorVm RequestConfiguratorVm { get; }

    public bool IsEditorVisible => IsSourceEditorOpen || IsRequestEditorOpen;

    public bool IsRequestWorkspaceVisible => !IsEditorVisible;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestWorkspaceVisible))]
    private bool _isSourceEditorOpen;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditorVisible))]
    [NotifyPropertyChangedFor(nameof(IsRequestWorkspaceVisible))]
    private bool _isRequestEditorOpen;

    [ObservableProperty]
    private ApiSourceModel? _selectedSource;

    [ObservableProperty]
    private ApiRequestModel? _selectedRequest;

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
        var wasSelectedSource = SelectedSource == source;

        _sourceManagementService.DeleteSource(source);

        if (!wasSelectedSource)
        {
            return;
        }

        SelectedSource = null;
        SelectedRequest = null;

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

        var wasSelectedRequest = SelectedRequest == request;

        _sourceManagementService.DeleteRequest(source, request);

        if (wasSelectedRequest)
        {
            SelectedRequest = null;
        }

        if (source.ApiRequests.Count == 0)
        {
            source.IsExpanded = false;
            SelectedSource = source;
            OpenSourceEditor();
        }
    }

    [RelayCommand]
    private void NavigateSourceContent(SourceNavigationPosition position)
    {
        OpenRequestWorkspace(position);
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
}