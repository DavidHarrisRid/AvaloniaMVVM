using System.Collections.Generic;

namespace AvaloniaMVVM.Models;

public class ApiRequestModel
{
    public int? ApiRequestId { get; private set; } 
    public int? ApiSourceId { get; private set; }
    public string? Name { get; private set; }
    public string? EndPointPath { get; private set; }
    public string? HttpMethod { get; private set; }
    public string? QueryStringParameters { get; private set; }
    public string? RequestBody { get; private set; }
    public string? RequestHeader { get; private set; }
    public int? PollingInterval { get; private set; }
    public bool? IsActive { get; private set; }
    
    public List<JsonEntryModel>? JsonEntries { get; private set; }
    
}