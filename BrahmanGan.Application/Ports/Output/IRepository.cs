using BrahmanGan.Domain.Common;

namespace BrahmanGan.Application.Ports.Output;

/// <summary>
/// Repositorio genérico para entidades del dominio.
/// </summary>
public interface IRepository<TEntity, TId>
    where TEntity : class
    where TId : Identity<int>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
