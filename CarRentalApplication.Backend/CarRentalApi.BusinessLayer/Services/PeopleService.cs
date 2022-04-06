using CarRentalApi.DataAccessLayer;

namespace CarRentalApi.BusinessLayer.Services;

public class PeopleService : IPeopleService
{
    private readonly IDataContext dataContext;

    public PeopleService(IDataContext dataContext)
    {
        this.dataContext = dataContext;
    }
}