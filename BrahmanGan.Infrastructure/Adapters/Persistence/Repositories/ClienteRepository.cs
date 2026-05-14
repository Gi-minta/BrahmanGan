using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 6 =====
public sealed class ClienteRepository : RepositoryBase<Cliente, ClienteId>, IClienteRepository
{
    public ClienteRepository(ApplicationDbContext db) : base(db) { }
    public Task<Cliente?> GetByDocumentoAsync(string documento, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(c => c.Documento == documento, ct);
}
