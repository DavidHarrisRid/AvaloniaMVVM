using AvaloniaMVVM.Models;
using AvaloniaMVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaMVVM.ViewModels.Menu.SourceContent;

public partial class RequestConfiguratorVm : BaseVm
{
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

    [ObservableProperty]
    private string? _saveMessage;

    public void Load(ApiRequestModel? request)
    {
        _request = request;

        Name = request?.Name;
        EndPointPath = request?.EndPointPath;
        HttpMethod = request?.HttpMethod;
        QueryStringParameters = request?.QueryStringParameters;
        RequestBody = request?.RequestBody;
        RequestHeader = request?.RequestHeader;
        PollingInterval = request?.PollingInterval?.ToString();
        IsActive = request?.IsActive;

        SaveMessage = null;
    }

    [RelayCommand]
    private void Save()
    {
        if (_request is null)
        {
            SaveMessage = "No request selected.";
            return;
        }

        _request.Name = Name;
        _request.EndPointPath = EndPointPath;
        _request.HttpMethod = HttpMethod;
        _request.QueryStringParameters = QueryStringParameters;
        _request.RequestBody = RequestBody;
        _request.RequestHeader = RequestHeader;
        _request.IsActive = IsActive;

        if (int.TryParse(PollingInterval, out var pollingInterval))
        {
            _request.PollingInterval = pollingInterval;
        }
        else
        {
            _request.PollingInterval = null;
        }

        SaveMessage = "Request saved.";
    }
}