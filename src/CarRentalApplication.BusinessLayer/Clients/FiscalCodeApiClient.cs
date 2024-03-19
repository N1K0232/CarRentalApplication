using System.Net.Http.Json;
using CarRentalApplication.BusinessLayer.Clients.Interfaces;
using CarRentalApplication.BusinessLayer.Settings;
using CarRentalApplication.Shared.Models.Requests;
using CarRentalApplication.Shared.Models.Responses;
using Microsoft.Extensions.Options;

namespace CarRentalApplication.BusinessLayer.Clients;

public class FiscalCodeApiClient : IFiscalCodeApiClient
{
    private readonly HttpClient client;
    private readonly FiscalCodeApiSettings fiscalCodeApiSettings;

    public FiscalCodeApiClient(HttpClient client, IOptions<FiscalCodeApiSettings> fiscalCodeApiSettingsOptions)
    {
        this.client = client;
        fiscalCodeApiSettings = fiscalCodeApiSettingsOptions.Value;
    }

    public async Task<FiscalCodeResponse> CalculateAsync(FiscalCodeRequest request)
    {
        var requestUri = $"calculate?lname={request.LastName}&fname={request.FirstName}" +
            $"&gender={request.Gender}&city={request.City}&state={request.Province}" +
            $"&abolished={request.Abolished}&day={request.Day}&month={request.Month}&year={request.Year}" +
            $"&omocodia_level={request.OmocodiaLevel}&access_token={fiscalCodeApiSettings.ApiKey}";

        var response = await client.GetFromJsonAsync<FiscalCodeResponse>(requestUri);
        return response;
    }
}