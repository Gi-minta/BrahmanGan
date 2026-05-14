using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ContratoRepository : RepositoryBase<Contrato, ContratoId>, IContratoRepository
{
    public ContratoRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<Contrato>> ListByClienteAsync(ClienteId idCliente, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(c => c.IdCliente == idCliente).ToListAsync(ct);
}
