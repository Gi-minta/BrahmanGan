using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.Ports.Output;

public interface IControlPreventivoRepository : IRepository<ControlPreventivo, ControlPreventivoId>
{
    Task<IReadOnlyList<ControlPreventivo>> ListAllAsync(CancellationToken ct = default);
}
