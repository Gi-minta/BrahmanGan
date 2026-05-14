using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

/// <summary>Base genérica para repositorios EF Core.</summary>
public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
    where TId : Identity<int>
{
    protected readonly ApplicationDbContext Db;
    protected DbSet<TEntity> Set => Db.Set<TEntity>();

    protected RepositoryBase(ApplicationDbContext db) { Db = db; }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default)
        => await Set.FindAsync(new object[] { id }, ct);

    public virtual async Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().ToListAsync(ct);

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
        => await Set.AddAsync(entity, ct);

    public virtual void Update(TEntity entity) => Set.Update(entity);
    public virtual void Remove(TEntity entity) => Set.Remove(entity);
}
