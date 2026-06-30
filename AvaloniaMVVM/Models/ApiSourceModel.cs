using System.Collections.ObjectModel;

namespace AvaloniaMVVM.Models;

public class ApiSourceModel
{
    public int? ApiSourceId { get; set; }
    public string? Name { get; set; }
    public string? DefaultBaseUrl { get; set; }
    public string? Description { get; set; }
    public string? AuthenticationType { get; set; }
    public string? ApiKey { get; set; }
    public string? DefaultHeader { get; set; }

    public ObservableCollection<ApiRequestModel> ApiRequests { get; } = new();
}