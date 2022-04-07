using CarRentalApi.DataAccessLayer.Entities.Common;

namespace CarRentalApi.DataAccessLayer.Entities;

public class Reservation : BaseEntity
{
    public Guid IdPerson { get; set; }

    public Guid IdVehicle { get; set; }

    public DateTime Start { get; set; }

    public DateTime Finish { get; set; }

    public virtual Person Person { get; set; }

    public virtual Vehicle Vehicle { get; set; }
}