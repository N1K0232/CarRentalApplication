using System.Text.Json.Serialization;

namespace CarRentalApplication.Shared.Models;

public class FiscalCodeData
{
    [JsonPropertyName("cf")]
    public string FiscalCode { get; set; } = null!;

    [JsonPropertyName("all_cf")]
    public string[] FiscalCodes { get; set; } = null!;
}