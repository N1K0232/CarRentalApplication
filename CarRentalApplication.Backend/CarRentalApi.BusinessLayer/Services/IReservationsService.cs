using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;

namespace CarRentalApi.BusinessLayer.Services;

public interface IReservationsService
{
    Task DeleteAsync(Guid id);
    Task DeleteAsync();
    Task<Reservation> GetAsync(Guid id);
    Task<Reservation> SaveAsync(SaveReservationRequest request);
}