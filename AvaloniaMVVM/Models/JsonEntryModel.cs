using System;

namespace AvaloniaMVVM.Models;

public class JsonEntryModel
{
    public int? JsonEntryId { get; private set; }
    public int? ApiRequest { get; private set; }
    public DateTime? FetchedAt { get; private set; }
    public int? StatusCode { get; private set; }
    public bool? IsSuccess { get; private set; }
    public string? RawResponse { get; private set; }
    public string? ErrorMessage { get; private set; }
}