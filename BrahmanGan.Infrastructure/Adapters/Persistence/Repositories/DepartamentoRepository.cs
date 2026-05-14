using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class DepartamentoRepository(ApplicationDbContext db) : RepositoryBase<Departamento, DepartamentoId>(db), IDepartamentoRepository;
