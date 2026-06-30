using System.Collections.Generic;

namespace AvaloniaMVVM.Models;

public class ApiSourceModel
{
    public int? ApiSourceId { get; private set; }
    public string? Name { get; private set; }
    public string? DefaultBaseUrl { get; private set; }
    public string? Description { get; private set; }
    public string? AuthenticationType { get; private set; }
    public string? ApiKey { get; private set; }
    public string? DefaultHeader { get; private set; }
    
    public List<ApiRequestModel>? ApiRequests { get; private set; }
}