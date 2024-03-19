using CarRentalApplication.Shared.Models.Requests;
using CarRentalApplication.Shared.Models.Responses;

namespace CarRentalApplication.BusinessLayer.Clients.Interfaces;

public interface IFiscalCodeApiClient
{
    Task<FiscalCodeResponse> CalculateAsync(FiscalCodeRequest request);
}