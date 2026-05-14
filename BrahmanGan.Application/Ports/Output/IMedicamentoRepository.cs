using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.Ports.Output;

// Fase 4
public interface IMedicamentoRepository : IRepository<Medicamento, MedicamentoId> { }
