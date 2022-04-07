using AutoMapper;
using CarRentalApi.DataAccessLayer;
using CarRentalApi.Shared.Models;
using CarRentalApi.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = CarRentalApi.DataAccessLayer.Entities;

namespace CarRentalApi.BusinessLayer.Services;

public class VehiclesService : IVehiclesService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public VehiclesService(IDataContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }

    public async Task DeleteAsync()
    {
        var query = dataContext.GetData<Entities.Vehicle>();
        var dbVehicles = await query.ToListAsync();
        dataContext.Delete(dbVehicles);
        await dataContext.SaveAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var dbVehicle = await dataContext.GetAsync<Entities.Vehicle>(id);
        dataContext.Delete(dbVehicle);
        await dataContext.SaveAsync();
    }
    public async Task<Vehicle> GetAsync(Guid id)
    {
        var dbVehicle = await dataContext.GetAsync<Entities.Vehicle>(id);
        if (dbVehicle == null)
        {
            return null;
        }

        var vehicle = mapper.Map<Vehicle>(dbVehicle);
        return vehicle;
    }
    public async Task<Vehicle> SaveAsync(SaveVehicleRequest request)
    {
        var query = dataContext.GetData<Entities.Vehicle>(trackingChanges: true);
        var dbVehicle = request.Id != null ?
            await query.FirstOrDefaultAsync(v => v.Id == request.Id) : null;

        if (dbVehicle == null)
        {
            dbVehicle = mapper.Map<Entities.Vehicle>(request);
            dataContext.Insert(dbVehicle);
        }
        else
        {
            mapper.Map(request, dbVehicle);
            dbVehicle.LastModifiedDate = DateTime.UtcNow;
        }

        await dataContext.SaveAsync();
        return mapper.Map<Vehicle>(dbVehicle);
    }
}