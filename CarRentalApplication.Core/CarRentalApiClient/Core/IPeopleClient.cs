using CarRentalApi.Shared.Models.Requests;

namespace CarRentalApiClient.Core;

public interface IPeopleClient : IDisposable
{
    Task<string> SaveAsync(SavePersonRequest request);
    Task<string> UpdateAsync(SavePersonRequest request);
}