using CarRentalApi.Shared.Models.Requests;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace CarRentalApiClient.Core;

public class PeopleClient : IPeopleClient, IDisposable
{
    private readonly HttpClient httpClient;

    public PeopleClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    ~PeopleClient()
    {
        Dispose(false);
    }

    public async Task<string> SaveAsync(SavePersonRequest request)
    {
        using var apiResponse = await httpClient.PostAsJsonAsync(Constants.SavePerson, request);
        if (apiResponse.IsSuccessStatusCode)
        {
            var content = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<string>(content);
            return response;
        }

        return null;
    }
    public async Task<string> UpdateAsync(SavePersonRequest request)
    {
        using var apiResponse = await httpClient.PutAsJsonAsync(Constants.UpdatePerson, request);
        if (apiResponse.IsSuccessStatusCode)
        {
            var content = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<string>(content);
            return response;
        }

        return null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposing && httpClient != null)
        {
            httpClient.Dispose();
        }
    }
}