using AutoMapper;
using CarRentalApi.DataAccessLayer;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = CarRentalApi.DataAccessLayer.Entities;

namespace CarRentalApi.BusinessLayer.Services;

public class ReservationsService : IReservationsService
{
    private readonly IPeopleService peopleService;
    private readonly IVehiclesService vehiclesService;
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public ReservationsService(IPeopleService peopleService, IVehiclesService vehiclesService, IDataContext dataContext, IMapper mapper)
    {
        this.peopleService = peopleService;
        this.vehiclesService = vehiclesService;
        this.dataContext = dataContext;
        this.mapper = mapper;
    }

    public async Task DeleteAsync()
    {
        var query = dataContext.GetData<Entities.Reservation>();
        var dbReservations = await query.ToListAsync();
        if (dbReservations != null && dbReservations.Count > 0)
        {
            dataContext.Delete(dbReservations);
            await dataContext.SaveAsync();
        }
    }
    public async Task DeleteAsync(Guid id)
    {
        var dbReservation = await dataContext.GetAsync<Entities.Reservation>(id);
        if (dbReservation != null)
        {
            dataContext.Delete(dbReservation);
            await dataContext.SaveAsync();
        }
    }
    public async Task<Reservation> GetAsync(Guid id)
    {
        var dbReservation = await dataContext.GetAsync<Entities.Reservation>(id);
        if (dbReservation != null)
        {
            var person = await peopleService.GetPersonAsync(dbReservation.IdPerson);
            var vehicle = await vehiclesService.GetVehicleAsync(dbReservation.IdVehicle);
            var reservation = mapper.Map<Reservation>(dbReservation);
            reservation.Person = person;
            reservation.Vehicle = vehicle;
            return reservation;
        }

        return null;
    }
    public async Task<Reservation> SaveAsync(SaveReservationRequest request)
    {
        var query = dataContext.GetData<Entities.Reservation>(trackingChanges: true);
        var dbReservation = request.Id != null ?
            await query.FirstOrDefaultAsync(r => r.Id == request.Id) : null;

        if (dbReservation == null)
        {
            dbReservation = mapper.Map<Entities.Reservation>(request);
            dataContext.Insert(dbReservation);
        }
        else
        {
            mapper.Map(request, dbReservation);
            dbReservation.LastModifiedDate = DateTime.UtcNow;
        }

        await dataContext.SaveAsync();
        return mapper.Map<Reservation>(dbReservation);
    }
}