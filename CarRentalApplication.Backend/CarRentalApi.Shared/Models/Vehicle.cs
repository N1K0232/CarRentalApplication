using CarRentalApi.Shared.Models.Common;

namespace CarRentalApi.Shared.Models;

public class Vehicle : BaseObject
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }
}