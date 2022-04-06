using CarRentalApi.Shared.Models.Common;
using CarRentalApi.Shared.Models.Enums;

namespace CarRentalApi.Shared.Models;

public class Vehicle : BaseObject
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }

    public PowerSupplyType PowerSupplyType { get; set; }
}