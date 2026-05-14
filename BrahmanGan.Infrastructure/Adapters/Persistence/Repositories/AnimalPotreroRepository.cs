using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class AnimalPotreroRepository : RepositoryBase<AnimalPotrero, AnimalPotreroId>, IAnimalPotreroRepository
{
    public AnimalPotreroRepository(ApplicationDbContext db) : base(db) { }
    public Task<AnimalPotrero?> GetVigenteByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(ap => ap.IdAnimal == idAnimal && ap.FechaSalida == null, ct);
}
