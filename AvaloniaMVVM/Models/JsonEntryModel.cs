using System;

namespace AvaloniaMVVM.Models;

public class JsonEntryModel
{
    public int? JsonEntryId { get; set; }
    public int? ApiRequest { get; set; }
    public DateTime? FetchedAt { get; set; }
    public int? StatusCode { get; set; }
    public bool? IsSuccess { get; set; }
    public string? RawResponse { get; set; }
    public string? ErrorMessage { get; set; }
}