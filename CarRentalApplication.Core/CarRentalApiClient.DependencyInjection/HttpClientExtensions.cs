using CarRentalApiClient.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CarRentalApiClient.DependencyInjection;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder AddPersonClient(this IServiceCollection services)
    {
        return services.AddHttpClient<IPeopleClient, PeopleClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return new PeopleClient(httpClient);
        });
    }
}