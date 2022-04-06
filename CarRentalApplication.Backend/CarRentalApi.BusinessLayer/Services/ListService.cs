using AutoMapper;
using CarRentalApi.DataAccessLayer;
using CarRentalApi.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Entities = CarRentalApi.DataAccessLayer.Entities;

namespace CarRentalApi.BusinessLayer.Services;

public class ListService : IListService
{
    private readonly IReadOnlyDataContext dataContext;
    private readonly IMapper mapper;

    public ListService(IReadOnlyDataContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
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
    public async Task<List<Vehicle>> GetVehiclesAsync()
    {
        var query = dataContext.GetData<Entities.Vehicle>();
        var dbVehicles = await query.ToListAsync();
        if (dbVehicles == null || dbVehicles.Count == 0)
        {
            return null;
        }

        var vehicles = mapper.Map<List<Vehicle>>(dbVehicles);
        return vehicles;
    }
    public async Task<List<Reservation>> GetReservationsAsync()
    {
        var query = dataContext.GetData<Entities.Reservation>();
        var dbReservations = await query.ToListAsync();
        if (dbReservations == null || dbReservations.Count == 0)
        {
            return null;
        }

        var reservations = mapper.Map<List<Reservation>>(dbReservations);
        return reservations;
    }
}