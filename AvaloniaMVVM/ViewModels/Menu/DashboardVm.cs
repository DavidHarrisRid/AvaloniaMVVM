using System.Collections.ObjectModel;
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
    [NotifyPropertyChangedFor(nameof(Requests))]
    [NotifyCanExecuteChangedFor(nameof(CreateRequestCommand))]
    private ApiSourceModel? _selectedSource;

    [ObservableProperty]
    private ApiRequestModel? _selectedRequest;

    public ObservableCollection<ApiRequestModel>? Requests => SelectedSource?.ApiRequests;

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

        OpenSourceEditor();
    }

    [RelayCommand(CanExecute = nameof(CanCreateRequest))]
    private void CreateRequest()
    {
        if (SelectedSource is null)
        {
            return;
        }

        SelectedRequest = _sourceManagementService.CreateRequest(SelectedSource);

        OpenRequestEditor();
    }

    private bool CanCreateRequest()
    {
        return SelectedSource is not null;
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
    private void OpenRequest(ApiRequestModel request)
    {
        SelectedRequest = request;

        OpenRequestWorkspace(SourceNavigationPosition.DataDashboard);
    }

    [RelayCommand]
    private void EditRequest(ApiRequestModel request)
    {
        SelectedRequest = request;

        OpenRequestEditor();
    }

    [RelayCommand]
    private void DeleteRequest(ApiRequestModel request)
    {
        if (SelectedSource is null)
        {
            return;
        }

        var wasSelectedRequest = SelectedRequest == request;

        _sourceManagementService.DeleteRequest(SelectedSource, request);

        if (wasSelectedRequest)
        {
            SelectedRequest = null;
        }

        if (SelectedSource.ApiRequests.Count == 0)
        {
            OpenSourceEditor();
        }
    }

    [RelayCommand]
    private void NavigateSourceContent(SourceNavigationPosition position)
    {
        OpenRequestWorkspace(position);
    }

    private void OpenSourceEditor()
    {
        IsSourceEditorOpen = true;
        IsRequestEditorOpen = false;
    }

    private void OpenRequestEditor()
    {
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