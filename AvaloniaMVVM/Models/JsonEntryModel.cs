using System;

namespace AvaloniaMVVM.Models;

public class JsonEntryModel
{
    // Ein Eintrag beschreibt das Ergebnis eines späteren API-Abrufs einschließlich Fehlerdaten.
    public int? JsonEntryId { get; set; }
    public int? ApiRequest { get; set; }
    public DateTime? FetchedAt { get; set; }
    public int? StatusCode { get; set; }
    public bool? IsSuccess { get; set; }
    public string? RawResponse { get; set; }
    public string? ErrorMessage { get; set; }
}
