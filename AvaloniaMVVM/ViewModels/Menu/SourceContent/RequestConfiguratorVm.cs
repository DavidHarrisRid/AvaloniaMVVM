using AvaloniaMVVM.Models;
using AvaloniaMVVM.Services;
using AvaloniaMVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu.SourceContent;

public partial class RequestConfiguratorVm : BaseVm
{
    private readonly RequestConfigurationService _requestConfigurationService;

    private ApiRequestModel? _request;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _endPointPath;

    [ObservableProperty]
    private string? _httpMethod;

    [ObservableProperty]
    private string? _queryStringParameters;

    [ObservableProperty]
    private string? _requestBody;

    [ObservableProperty]
    private string? _requestHeader;

    [ObservableProperty]
    private string? _pollingInterval;

    [ObservableProperty]
    private bool? _isActive;

    public RequestConfiguratorVm(RequestConfigurationService requestConfigurationService)
    {
        _requestConfigurationService = requestConfigurationService;
    }

    public void Load(ApiRequestModel? request)
    {
        // Das Laden fuellt nur das Formular und veraendert das Modell noch nicht.
        _request = request;

        _requestConfigurationService.Load(
            request,
            out var name,
            out var endPointPath,
            out var httpMethod,
            out var queryStringParameters,
            out var requestBody,
            out var requestHeader,
            out var pollingInterval,
            out var isActive);

        Name = name;
        EndPointPath = endPointPath;
        HttpMethod = httpMethod;
        QueryStringParameters = queryStringParameters;
        RequestBody = requestBody;
        RequestHeader = requestHeader;
        PollingInterval = pollingInterval;
        IsActive = isActive;
    }

    [RelayCommand]
    private void Save()
    {
        _requestConfigurationService.Save(
            _request,
            Name,
            EndPointPath,
            HttpMethod,
            QueryStringParameters,
            RequestBody,
            RequestHeader,
            PollingInterval,
            IsActive);
    }
}
