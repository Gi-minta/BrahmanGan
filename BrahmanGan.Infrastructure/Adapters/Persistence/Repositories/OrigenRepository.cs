using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class OrigenRepository(ApplicationDbContext db) : RepositoryBase<Origen, OrigenId>(db), IOrigenRepository;
