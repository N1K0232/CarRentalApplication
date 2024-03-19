using AutoMapper;
using CarRentalApplication.Shared.Models;
using CarRentalApplication.Shared.Models.Requests;
using Entities = CarRentalApplication.DataAccessLayer.Entities;

namespace CarRentalApplication.BusinessLayer.MapperProfiles;

public class PersonMapperProfile : Profile
{
    public PersonMapperProfile()
    {
        CreateMap<Entities.Person, Person>();
        CreateMap<SavePersonRequest, Entities.Person>();
    }
}