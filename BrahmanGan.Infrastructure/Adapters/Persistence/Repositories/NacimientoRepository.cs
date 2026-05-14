using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class NacimientoRepository(ApplicationDbContext db) : RepositoryBase<Nacimiento, NacimientoId>(db), INacimientoRepository;
