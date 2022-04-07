using CarRentalApi.Shared.Models.Requests;
using CarRentalApiClient;
using CarRentalApiClient.Core;
using System.Net.Http.Headers;

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri(Constants.BaseUrl);
httpClient.DefaultRequestHeaders.Accept.Clear();
httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
using var peopleClient = new PeopleClient(httpClient);

var request = new SavePersonRequest
{
    FirstName = "Nicola",
    LastName = "Silvestri",
    BirthDate = DateTime.Parse("22/10/2002"),
    PhoneNumber = "3319907702",
    EmailAddress = "ns.nicolasilvestri@gmail.com"
};

var response = await peopleClient.SaveAsync(request);

Console.ReadLine();