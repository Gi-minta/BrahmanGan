using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 1 =====
public sealed class AnimalRepository : RepositoryBase<Animal, AnimalId>, IAnimalRepository
{
    public AnimalRepository(ApplicationDbContext db) : base(db) { }
    public Task<Animal?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(a => a.Codigo == codigo, ct);
    public async Task<IReadOnlyList<Animal>> ListByFincaAsync(FincaId idFinca, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(a => a.IdFinca == idFinca).ToListAsync(ct);
    public async Task<IReadOnlyList<Animal>> ListActivosAsync(CancellationToken ct = default)
        => await Set.AsNoTracking().Where(a => a.Estado == EstadoAnimal.ACTIVO).ToListAsync(ct);
}
