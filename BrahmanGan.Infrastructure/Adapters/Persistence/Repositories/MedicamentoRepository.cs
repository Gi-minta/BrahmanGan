using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

// ===== Fase 4 =====
public sealed class MedicamentoRepository(ApplicationDbContext db) : RepositoryBase<Medicamento, MedicamentoId>(db), IMedicamentoRepository;
