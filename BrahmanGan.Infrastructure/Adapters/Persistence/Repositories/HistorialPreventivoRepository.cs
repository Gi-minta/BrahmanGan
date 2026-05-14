using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialPreventivoRepository(ApplicationDbContext db) : RepositoryBase<HistorialPreventivo, HistorialPreventivoId>(db), IHistorialPreventivoRepository;
