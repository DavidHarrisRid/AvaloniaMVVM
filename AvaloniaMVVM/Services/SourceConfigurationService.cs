using AvaloniaMVVM.Models;

namespace AvaloniaMVVM.Services;

public class SourceConfigurationService
{
    // Load und Save trennen die editierbaren Formularwerte vom gespeicherten Modellzustand.
    private readonly StatusBarService _statusBarService;

    public SourceConfigurationService(StatusBarService statusBarService)
    {
        _statusBarService = statusBarService;
    }

    public void Load(
        ApiSourceModel? source,
        out string? name,
        out string? defaultBaseUrl,
        out string? description,
        out string? authenticationType,
        out string? apiKey,
        out string? defaultHeader)
    {
        name = source?.Name;
        defaultBaseUrl = source?.DefaultBaseUrl;
        description = source?.Description;
        authenticationType = source?.AuthenticationType;
        apiKey = source?.ApiKey;
        defaultHeader = source?.DefaultHeader;
    }

    public void Save(
        ApiSourceModel? source,
        string? name,
        string? defaultBaseUrl,
        string? description,
        string? authenticationType,
        string? apiKey,
        string? defaultHeader)
    {
        if (source is null)
        {
            _statusBarService.SetMessage("No source selected.");
            return;
        }

        source.Name = name;
        source.DefaultBaseUrl = defaultBaseUrl;
        source.Description = description;
        source.AuthenticationType = authenticationType;
        source.ApiKey = apiKey;
        source.DefaultHeader = defaultHeader;

        _statusBarService.SetMessage("Source saved.");
    }
}
