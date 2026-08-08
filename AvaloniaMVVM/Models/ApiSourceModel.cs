using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMVVM.Models;

public partial class ApiSourceModel : ObservableObject
{
    // ObservableProperty erzeugt bindbare Properties und Benachrichtigungen für die Oberfläche.
    [ObservableProperty]
    private int? _apiSourceId;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _defaultBaseUrl;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _authenticationType;

    [ObservableProperty]
    private string? _apiKey;

    [ObservableProperty]
    private string? _defaultHeader;

    [ObservableProperty]
    private bool _isExpanded;

    // Eine Quelle ist der übergeordnete Knoten für ihre API-Anfragen.
    public ObservableCollection<ApiRequestModel> ApiRequests { get; } = new();
}
