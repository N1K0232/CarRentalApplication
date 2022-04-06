using CarRentalApi.DataAccessLayer.Entities.Common;
using CarRentalApi.Shared.Models.Enums;

namespace CarRentalApi.DataAccessLayer.Entities;

public class Vehicle : BaseEntity
{
    public string Brand { get; set; }

    public string Model { get; set; }

    public string Plate { get; set; }

    public PowerSupplyType PowerSupplyType { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; }
}