using CarRentalApplication.BusinessLayer.Services.Interfaces;
using CarRentalApplication.Shared.Models;
using CarRentalApplication.Shared.Models.Common;
using CarRentalApplication.Shared.Models.Requests;
using MinimalHelpers.Routing;
using OperationResults.AspNetCore.Http;

namespace CarRentalApplication.Endpoints;

public class PeopleEndpoint : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var peopleApiGroup = endpoints.MapGroup("api/people/");

        peopleApiGroup.MapDelete("{id:guid}", DeleteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        peopleApiGroup.MapGet("{id:guid}", GetAsync)
            .WithName("GetPerson")
            .Produces(StatusCodes.Status200OK, typeof(Person))
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        peopleApiGroup.MapGet(string.Empty, GetListAsync)
            .Produces(StatusCodes.Status200OK, typeof(ListResult<Person>))
            .Produces(StatusCodes.Status400BadRequest);

        peopleApiGroup.MapPost(string.Empty, InsertAsync)
            .Produces(StatusCodes.Status201Created, typeof(Person))
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        peopleApiGroup.MapPut("{id:guid}", UpdateAsync)
            .Produces(StatusCodes.Status200OK, typeof(Person))
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> DeleteAsync(IPeopleService peopleService, Guid id, HttpContext context)
    {
        var result = await peopleService.DeleteAsync(id);
        return context.CreateResponse(result);
    }

    private static async Task<IResult> GetAsync(IPeopleService peopleService, Guid id, HttpContext context)
    {
        var result = await peopleService.GetAsync(id);
        return context.CreateResponse(result);
    }

    private static async Task<IResult> GetListAsync(IPeopleService peopleService, HttpContext context, string searchText = null, string orderBy = "FirstName, LastName", int pageIndex = 0, int itemsPerPage = 10)
    {
        var result = await peopleService.GetListAsync(searchText, orderBy, pageIndex, itemsPerPage);
        return context.CreateResponse(result);
    }

    private static async Task<IResult> InsertAsync(IPeopleService peopleService, SavePersonRequest person, HttpContext context)
    {
        var result = await peopleService.InsertAsync(person);
        return context.CreateResponse(result, "GetPerson", new { id = result.Content?.Id });
    }

    private static async Task<IResult> UpdateAsync(IPeopleService peopleService, Guid id, SavePersonRequest person, HttpContext context)
    {
        var result = await peopleService.UpdateAsync(id, person);
        return context.CreateResponse(result);
    }
}