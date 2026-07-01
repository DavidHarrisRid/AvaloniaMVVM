using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Models;

public partial class ApiRequestModel : ObservableObject
{
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

    public ObservableCollection<JsonEntryModel> JsonEntries { get; } = new();
}