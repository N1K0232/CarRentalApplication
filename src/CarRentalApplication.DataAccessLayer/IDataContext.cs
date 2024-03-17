﻿using CarRentalApplication.DataAccessLayer.Entities.Common;

namespace CarRentalApplication.DataAccessLayer;

public interface IDataContext
{
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

    IQueryable<TEntity> Get<TEntity>(bool trackingChanges = false) where TEntity : BaseEntity;

    ValueTask<TEntity> GetAsync<TEntity>(Guid id) where TEntity : BaseEntity;

    Task InsertAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task<int> SaveAsync();

    Task ExecuteTransactionAsync(Func<Task> action);
}