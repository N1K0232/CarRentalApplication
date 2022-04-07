using CarRentalApi.Shared.Models.Common;

namespace CarRentalApi.Shared.Models.Requests;

public class SaveReservationRequest : BaseRequestObject
{
    public Guid IdPerson { get; set; }

    public Guid IdVehicle { get; set; }

    public DateTime Start { get; set; }

    public DateTime Finish { get; set; }
}