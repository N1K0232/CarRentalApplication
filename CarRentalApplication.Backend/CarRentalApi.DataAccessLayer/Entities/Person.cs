using CarRentalApi.DataAccessLayer.Entities.Common;

namespace CarRentalApi.DataAccessLayer.Entities;

public class Person : BaseEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string EmailAddress { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; }
}