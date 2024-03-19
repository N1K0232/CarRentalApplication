using System.Text.Json.Serialization;

namespace CarRentalApplication.Shared.Models.Responses;

public class FiscalCodeResponse
{
    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("data")]
    public FiscalCodeData Data { get; set; } = null!;
}