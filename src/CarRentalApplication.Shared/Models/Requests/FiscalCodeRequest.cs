using System.Text.Json.Serialization;

namespace CarRentalApplication.Shared.Models.Requests;

public class FiscalCodeRequest
{
    [JsonPropertyName("fname")]
    public string FirstName { get; set; } = null!;

    [JsonPropertyName("lname")]
    public string LastName { get; set; } = null!;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = null!;

    [JsonPropertyName("city")]
    public string City { get; set; } = null!;

    [JsonPropertyName("status")]
    public string Province { get; set; } = null!;

    [JsonPropertyName("abolished")]
    public bool Abolished { get; set; }

    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("omocodia_level")]
    public int OmocodiaLevel { get; set; }
}