using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;

namespace BrahmanGan.Infrastructure.Adapters.Persistence;

/// <summary>
/// Implementación del patrón Unit of Work.
/// Tras guardar cambios despacha los eventos de dominio acumulados en las entidades.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IDomainEventDispatcher _dispatcher;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ApplicationDbContext context, IDomainEventDispatcher dispatcher)
    {
        _context = context;
        _dispatcher = dispatcher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var events = ExtraerEventosDeDominio();
        try
        {
            var result = await _context.SaveChangesAsync(ct);
            if (events.Count > 0)
                await _dispatcher.DispatchAsync(events, ct);
            return result;
        }
        finally
        {
            // Always clear so consecutive operations with transient keys (RazaId(0),
            // AnimalId(0), etc.) don't collide, including when SaveChanges throws
            // (e.g. unique-constraint violation during bulk import).
            _context.ChangeTracker.Clear();
        }
    }

    private List<IDomainEvent> ExtraerEventosDeDominio()
    {
        var entries = _context.ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvents>()
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var events = entries.SelectMany(e => e.DomainEvents).ToList();
        foreach (var e in entries) e.ClearDomainEvents();
        return events;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress");
        }

        try
        {
            await _context.SaveChangesAsync(ct);
            await _currentTransaction.CommitAsync(ct);
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress");
        }

        try
        {
            await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
