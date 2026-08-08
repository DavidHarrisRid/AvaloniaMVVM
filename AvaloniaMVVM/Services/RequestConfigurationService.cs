using AvaloniaMVVM.Models;

namespace AvaloniaMVVM.Services;

public class RequestConfigurationService
{
    // Erst Save uebertraegt die Formularwerte in das ausgewaehlte Request-Modell.
    private readonly StatusBarService _statusBarService;

    public RequestConfigurationService(StatusBarService statusBarService)
    {
        _statusBarService = statusBarService;
    }

    public void Load(
        ApiRequestModel? request,
        out string? name,
        out string? endPointPath,
        out string? httpMethod,
        out string? queryStringParameters,
        out string? requestBody,
        out string? requestHeader,
        out string? pollingInterval,
        out bool? isActive)
    {
        name = request?.Name;
        endPointPath = request?.EndPointPath;
        httpMethod = request?.HttpMethod;
        queryStringParameters = request?.QueryStringParameters;
        requestBody = request?.RequestBody;
        requestHeader = request?.RequestHeader;
        pollingInterval = request?.PollingInterval?.ToString();
        isActive = request?.IsActive;
    }

    public void Save(
        ApiRequestModel? request,
        string? name,
        string? endPointPath,
        string? httpMethod,
        string? queryStringParameters,
        string? requestBody,
        string? requestHeader,
        string? pollingInterval,
        bool? isActive)
    {
        if (request is null)
        {
            _statusBarService.SetMessage("No request selected.");
            return;
        }

        request.Name = name;
        request.EndPointPath = endPointPath;
        request.HttpMethod = httpMethod;
        request.QueryStringParameters = queryStringParameters;
        request.RequestBody = requestBody;
        request.RequestHeader = requestHeader;
        request.IsActive = isActive;

        // Nichtnumerische oder leere Eingaben deaktivieren das optionale Polling-Intervall.
        if (int.TryParse(pollingInterval, out var parsedPollingInterval))
        {
            request.PollingInterval = parsedPollingInterval;
        }
        else
        {
            request.PollingInterval = null;
        }

        _statusBarService.SetMessage("Request saved.");
    }
}
