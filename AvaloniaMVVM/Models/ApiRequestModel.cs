using System.Collections.ObjectModel;

namespace AvaloniaMVVM.Models;

public class ApiRequestModel
{
    public int? ApiRequestId { get; set; }
    public int? ApiSourceId { get; set; }
    public string? Name { get; set; }
    public string? EndPointPath { get; set; }
    public string? HttpMethod { get; set; }
    public string? QueryStringParameters { get; set; }
    public string? RequestBody { get; set; }
    public string? RequestHeader { get; set; }
    public int? PollingInterval { get; set; }
    public bool? IsActive { get; set; }

    public ObservableCollection<JsonEntryModel> JsonEntries { get; } = new();
}