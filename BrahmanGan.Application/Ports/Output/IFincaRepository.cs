using BrahmanGan.Domain.Common;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;

namespace BrahmanGan.Application.Ports.Output;

public interface IFincaRepository : IRepository<FincaEntity, FincaId> { }
