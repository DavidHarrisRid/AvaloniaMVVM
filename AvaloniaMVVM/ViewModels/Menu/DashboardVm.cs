using System.Collections.ObjectModel;
using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu;

public partial class DashboardVm : BaseVm
{
    private readonly SourceManagementService _sourceManagementService;

    public ObservableCollection<ApiSourceModel> Sources => _sourceManagementService.ApiSources;

    public SourceNavigationService SourceNavigationService { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Requests))]
    [NotifyCanExecuteChangedFor(nameof(CreateRequestCommand))]
    private ApiSourceModel? _selectedSource;

    [ObservableProperty]
    private ApiRequestModel? _selectedRequest;

    public ObservableCollection<ApiRequestModel>? Requests => SelectedSource?.ApiRequests;

    public DashboardVm(
        SourceManagementService sourceManagementService,
        SourceNavigationService sourceNavigationService)
    {
        _sourceManagementService = sourceManagementService;
        SourceNavigationService = sourceNavigationService;
    }

    [RelayCommand]
    private void CreateSource()
    {
        SelectedSource = _sourceManagementService.CreateSource();
    }

    [RelayCommand(CanExecute = nameof(CanCreateRequest))]
    private void CreateRequest()
    {
        if (SelectedSource is null)
        {
            return;
        }

        SelectedRequest = _sourceManagementService.CreateRequest(SelectedSource);
    }

    private bool CanCreateRequest()
    {
        return SelectedSource is not null;
    }

    [RelayCommand]
    private void SelectSource(ApiSourceModel source)
    {
        SelectedSource = source;
        SelectedRequest = null;
    }

    [RelayCommand]
    private void SelectRequest(ApiRequestModel request)
    {
        SelectedRequest = request;
        SourceNavigationService.Navigate(SourceNavigationPosition.Configurator);
    }

    [RelayCommand]
    private void NavigateSourceContent(SourceNavigationPosition position)
    {
        SourceNavigationService.Navigate(position);
    }
}