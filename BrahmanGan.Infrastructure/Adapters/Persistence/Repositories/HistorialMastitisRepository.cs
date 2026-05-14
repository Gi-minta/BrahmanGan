using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialMastitisRepository(ApplicationDbContext db) : RepositoryBase<HistorialMastitis, HistorialMastitisId>(db), IHistorialMastitisRepository;
