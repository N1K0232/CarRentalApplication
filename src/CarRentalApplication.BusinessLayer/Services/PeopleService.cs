using System.Linq.Dynamic.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRentalApplication.BusinessLayer.Clients.Interfaces;
using CarRentalApplication.BusinessLayer.Extensions;
using CarRentalApplication.BusinessLayer.Services.Interfaces;
using CarRentalApplication.DataAccessLayer;
using CarRentalApplication.Shared.Models;
using CarRentalApplication.Shared.Models.Common;
using CarRentalApplication.Shared.Models.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OperationResults;
using TinyHelpers.Extensions;
using Entities = CarRentalApplication.DataAccessLayer.Entities;

namespace CarRentalApplication.BusinessLayer.Services;

public class PeopleService : IPeopleService
{
    private readonly IDataContext dataContext;
    private readonly IFiscalCodeApiClient fiscalCodeApiClient;
    private readonly IMapper mapper;
    private readonly IValidator<SavePersonRequest> personValidator;

    public PeopleService(IDataContext dataContext,
        IFiscalCodeApiClient fiscalCodeApiClient,
        IMapper mapper,
        IValidator<SavePersonRequest> personValidator)
    {
        this.dataContext = dataContext;
        this.fiscalCodeApiClient = fiscalCodeApiClient;
        this.mapper = mapper;
        this.personValidator = personValidator;
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Result.Fail(FailureReasons.ClientError, "Invalid id value", "Please insert a valid id");
        }

        var person = await dataContext.GetAsync<Entities.Person>(id);
        if (person is not null)
        {
            await dataContext.DeleteAsync(person);
            await dataContext.SaveAsync();

            return Result.Ok();
        }

        return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
    }

    public async Task<Result<Person>> GetAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return Result.Fail(FailureReasons.ClientError, "Invalid id value", "Please insert a valid id");
        }

        var dbPerson = await dataContext.GetAsync<Entities.Person>(id);
        if (dbPerson is not null)
        {
            var person = mapper.Map<Person>(dbPerson);
            return person;
        }

        return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
    }

    public async Task<Result<ListResult<Person>>> GetListAsync(string searchText, string orderBy, int pageIndex, int itemsPerPage)
    {
        var query = dataContext.Get<Entities.Person>();

        if (searchText.HasValue())
        {
            query = query.Where(p => p.FirstName.Contains(searchText) || p.LastName.Contains(searchText));
        }

        if (orderBy.HasValue())
        {
            query = query.OrderBy(orderBy);
        }

        var totalCount = await query.CountAsync();
        var people = await query.Skip(pageIndex * itemsPerPage).Take(itemsPerPage + 1)
            .ProjectTo<Person>(mapper.ConfigurationProvider)
            .ToListAsync();

        var result = new ListResult<Person>
        {
            Content = people.Take(itemsPerPage),
            TotalCount = totalCount,
            HasNextPage = people.Count > itemsPerPage
        };

        return result;
    }

    public async Task<Result<Person>> InsertAsync(SavePersonRequest person)
    {
        var validationResult = await personValidator.ValidateAsync(person);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.ToValidationErrors();
            return Result.Fail(FailureReasons.ClientError, "Invalid request", "", validationErrors);
        }

        try
        {
            var exists = await ExistsAsync(person.FirstName, person.LastName, person.BirthDate);
            if (exists)
            {
                return Result.Fail(FailureReasons.Conflict, "This person already exists", "This person already exists");
            }

            var dbPerson = mapper.Map<Entities.Person>(person);
            dbPerson.FiscalCode = await CalculateFiscalCodeAsync(person);

            await dataContext.InsertAsync(dbPerson);
            var affectedRows = await dataContext.SaveAsync();

            if (affectedRows > 0)
            {
                var savedPerson = mapper.Map<Person>(dbPerson);
                return savedPerson;
            }

            return Result.Fail(FailureReasons.DatabaseError, "No person saved", "No rows updated");
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(FailureReasons.DatabaseError, ex);
        }
    }

    public async Task<Result<Person>> UpdateAsync(Guid id, SavePersonRequest person)
    {
        if (id == Guid.Empty)
        {
            return Result.Fail(FailureReasons.ClientError, "Invalid id value", "Please insert a valid id");
        }

        var validationResult = await personValidator.ValidateAsync(person);
        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors.ToValidationErrors();
            return Result.Fail(FailureReasons.ClientError, "Invalid request", "", validationErrors);
        }

        try
        {
            var query = dataContext.Get<Entities.Person>(true);
            var exists = await ExistsAsync(person.FirstName, person.LastName, person.BirthDate);

            if (exists)
            {
                return Result.Fail(FailureReasons.Conflict, "This person already exists", "This person already exists");
            }

            var dbPerson = await query.FirstOrDefaultAsync(p => p.Id == id);
            if (dbPerson is null)
            {
                return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
            }

            mapper.Map(person, dbPerson);
            var affectedRows = await dataContext.SaveAsync();

            if (affectedRows > 0)
            {
                var savedPerson = mapper.Map<Person>(dbPerson);
                return savedPerson;
            }

            return Result.Fail(FailureReasons.DatabaseError, "No person saved", "No rows updated");
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(FailureReasons.DatabaseError, ex);
        }
    }

    private async Task<bool> ExistsAsync(string firstName, string lastName, DateOnly birthDate)
    {
        var query = dataContext.Get<Entities.Person>();
        return await query.AnyAsync(p => p.FirstName == firstName && p.LastName == lastName && p.BirthDate == birthDate);
    }

    private async Task<string> CalculateFiscalCodeAsync(SavePersonRequest person)
    {
        var request = new FiscalCodeRequest
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            Day = person.BirthDate.Day,
            Month = person.BirthDate.Month,
            Year = person.BirthDate.Year,
            Gender = person.Gender[0].ToString(),
            City = person.City,
            Province = person.Province
        };

        var response = await fiscalCodeApiClient.CalculateAsync(request);
        return response.Status ? response.Data.FiscalCode : null;
    }
}