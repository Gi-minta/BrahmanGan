using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class ParametroLactanciaRepository(ApplicationDbContext db) : RepositoryBase<ParametroLactancia, ParametroLactanciaId>(db), IParametroLactanciaRepository;
