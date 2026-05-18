using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class PedigriRepository : RepositoryBase<Pedigri, PedigriId>, IPedigriRepository
{
    public PedigriRepository(ApplicationDbContext db) : base(db) { }

    public Task<Pedigri?> GetByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(p => p.IdAnimal == idAnimal, ct);
}
