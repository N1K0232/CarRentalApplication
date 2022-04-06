using CarRentalApi.Shared.Models.Common;

namespace CarRentalApi.Shared.Models;

public class Reservation : BaseObject
{
    public Person Person { get; set; }

    public Vehicle Vehicle { get; set; }

    public DateTime Start { get; set; }

    public DateTime Finish { get; set; }
}