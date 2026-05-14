using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.Ports.Output;

public interface IMarcacionRepository : IRepository<Marcacion, MarcacionId> { }
