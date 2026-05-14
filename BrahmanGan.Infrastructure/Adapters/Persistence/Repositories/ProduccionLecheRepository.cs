using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ProduccionLecheRepository : RepositoryBase<ProduccionLeche, ProduccionLecheId>, IProduccionLecheRepository
{
    public ProduccionLecheRepository(ApplicationDbContext db) : base(db) { }
    public Task<ProduccionLeche?> GetByFincaFechaAsync(FincaId idFinca, DateOnly fecha, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(p => p.IdFinca == idFinca && p.Fecha == fecha, ct);
}
