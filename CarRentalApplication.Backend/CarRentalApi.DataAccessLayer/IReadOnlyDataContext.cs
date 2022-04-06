using CarRentalApi.DataAccessLayer.Entities.Common;

namespace CarRentalApi.DataAccessLayer;

public interface IReadOnlyDataContext
{
    IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity;
    Task<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity;
}