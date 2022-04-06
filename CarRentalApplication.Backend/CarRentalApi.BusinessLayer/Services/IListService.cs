using CarRentalApi.Shared.Models;

namespace CarRentalApi.BusinessLayer.Services
{
    public interface IListService
    {
        Task<List<Person>> GetPeopleAsync();
        Task<List<Reservation>> GetReservationsAsync();
        Task<List<Vehicle>> GetVehiclesAsync();
    }
}