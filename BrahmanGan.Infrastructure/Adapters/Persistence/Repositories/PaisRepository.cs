using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 2 =====
public sealed class PaisRepository(ApplicationDbContext db) : RepositoryBase<Pais, PaisId>(db), IPaisRepository;
