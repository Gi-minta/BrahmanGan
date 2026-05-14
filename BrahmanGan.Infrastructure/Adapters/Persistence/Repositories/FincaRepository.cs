using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class FincaRepository(ApplicationDbContext db) : RepositoryBase<FincaEntity, FincaId>(db), IFincaRepository;
