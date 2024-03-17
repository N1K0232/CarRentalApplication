using System.Reflection;
using CarRentalApplication.DataAccessLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApplication.DataAccessLayer;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
    {
        Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async ValueTask<TEntity> GetAsync<TEntity>(Guid id) where TEntity : BaseEntity
    {
        var entity = await Set<TEntity>().FindAsync(id);
        return entity;
    }

    public IQueryable<TEntity> Get<TEntity>(bool trackingChanges = false) where TEntity : BaseEntity
    {
        var set = Set<TEntity>();
        return trackingChanges ? set.AsTracking() : set.AsNoTrackingWithIdentityResolution();
    }

    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        await Set<TEntity>().AddAsync(entity);
    }

    public async Task<int> SaveAsync()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => typeof(BaseEntity).IsAssignableFrom(e.Entity.GetType()))
            .ToList();

        foreach (var entry in entries.Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            var entity = entry.Entity as BaseEntity;

            if (entry.State is EntityState.Modified)
            {
                entity.LastModifiedDate = DateTime.UtcNow;
            }
        }

        return await SaveChangesAsync(true);
    }

    public async Task ExecuteTransactionAsync(Func<Task> action)
    {
        var strategy = Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await Database.BeginTransactionAsync();
            await action.Invoke();
            await transaction.CommitAsync();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}