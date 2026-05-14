using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 3 =====
public sealed class SemenRepository(ApplicationDbContext db) : RepositoryBase<Semen, SemenId>(db), ISemenRepository;
