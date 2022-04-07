using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;

namespace CarRentalApi.BusinessLayer.Services;

public interface IPeopleService
{
    Task DeleteAsync();
    Task DeleteAsync(Guid idPerson);
    Task<Person> GetAsync(Guid id);
    Task<Person> SaveAsync(SavePersonRequest request);
}