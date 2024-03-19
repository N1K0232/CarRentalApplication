using CarRentalApplication.Shared.Models;
using CarRentalApplication.Shared.Models.Common;
using CarRentalApplication.Shared.Models.Requests;
using OperationResults;

namespace CarRentalApplication.BusinessLayer.Services.Interfaces;

public interface IPeopleService
{
    Task<Result> DeleteAsync(Guid id);

    Task<Result<Person>> GetAsync(Guid id);

    Task<Result<ListResult<Person>>> GetListAsync(string searchText, string orderBy, int pageIndex, int itemsPerPage);

    Task<Result<Person>> InsertAsync(SavePersonRequest person);

    Task<Result<Person>> UpdateAsync(Guid id, SavePersonRequest person);
}