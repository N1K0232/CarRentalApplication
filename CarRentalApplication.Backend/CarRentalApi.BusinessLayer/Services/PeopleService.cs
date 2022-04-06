using AutoMapper;
using CarRentalApi.DataAccessLayer;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = CarRentalApi.DataAccessLayer.Entities;

namespace CarRentalApi.BusinessLayer.Services;

public class PeopleService : IPeopleService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public PeopleService(IDataContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }

    public async Task DeleteAsync()
    {
        var query = dataContext.GetData<Entities.Person>();
        var dbPeople = await query.ToListAsync();
        dataContext.Delete(dbPeople);
        await dataContext.SaveAsync();
    }
    public async Task DeleteAsync(Guid idPerson)
    {
        var dbPerson = await dataContext.GetAsync<Entities.Person>(idPerson);
        if (dbPerson != null)
        {
            dataContext.Delete(dbPerson);
            await dataContext.SaveAsync();
        }
    }
    public async Task<List<Person>> GetPeopleAsync()
    {
        var query = dataContext.GetData<Entities.Person>();
        var dbPeople = await query.ToListAsync();
        if (dbPeople == null || dbPeople.Count == 0)
        {
            return null;
        }

        var people = mapper.Map<List<Person>>(dbPeople);
        return people;
    }
    public async Task<Person> GetPersonAsync(Guid id)
    {
        var dbPerson = await dataContext.GetAsync<Entities.Person>(id);
        if (dbPerson != null)
        {
            var person = mapper.Map<Person>(dbPerson);
            return person;
        }

        return null;
    }
    public async Task<Person> SaveAsync(SavePersonRequest request)
    {
        var dbPerson = request.Id != null ?
            await dataContext.GetAsync<Entities.Person>(request.Id) :
            null;

        if (dbPerson == null)
        {
            dbPerson = mapper.Map<Entities.Person>(request);
            dataContext.Insert(dbPerson);
        }
        else
        {
            dbPerson.FirstName = request.FirstName;
            dbPerson.LastName = request.LastName;
            dbPerson.BirthDate = request.BirthDate;
            dbPerson.PhoneNumber = request.PhoneNumber;
            dbPerson.EmailAddress = request.EmailAddress;
            dbPerson.LastModifiedDate = DateTime.UtcNow;
        }

        await dataContext.SaveAsync();
        return mapper.Map<Person>(dbPerson);
    }
}