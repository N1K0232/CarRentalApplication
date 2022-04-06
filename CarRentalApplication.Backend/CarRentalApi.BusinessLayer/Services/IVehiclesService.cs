using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;

namespace CarRentalApi.BusinessLayer.Services;

public interface IVehiclesService
{
    Task DeleteAsync(Guid id);
    Task DeleteAsync();
    Task<Vehicle> GetVehicleAsync(Guid id);
    Task<Vehicle> SaveAsync(SaveVehicleRequest request);
}