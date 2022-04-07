using CarRentalApi.Shared.Models.Common;

namespace CarRentalApi.Shared.Models.Requests;

public class SaveVehicleRequest : BaseRequestObject
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }
}