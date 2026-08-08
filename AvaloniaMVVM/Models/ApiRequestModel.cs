using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Models;

public partial class ApiRequestModel : ObservableObject
{
    // Die Modellwerte werden direkt von Konfigurator und Navigation beobachtet.
    [ObservableProperty]
    private int? _apiRequestId;

    [ObservableProperty]
    private int? _apiSourceId;

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
    private int? _pollingInterval;

    [ObservableProperty]
    private bool? _isActive;

    // Später enthält diese Sammlung die abgerufenen Antworten der Anfrage.
    public ObservableCollection<JsonEntryModel> JsonEntries { get; } = new();
}
