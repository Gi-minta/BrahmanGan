using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Costo diario operativo asociado a un centro de costo.</summary>
public sealed class CostoDiario : Entity<CostoDiarioId>
{
    public DateOnly Fecha { get; private set; }
    public CentroCostoId IdCentro { get; private set; } = null!;
    public string TipoCosto { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public decimal Valor { get; private set; }

    private CostoDiario() { }
    public static CostoDiario Crear(DateOnly fecha, CentroCostoId idCentro, string tipoCosto, decimal valor, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(tipoCosto)) throw new DomainException("Tipo de costo requerido");
        if (valor < 0) throw new DomainException("Valor no negativo");
        return new CostoDiario { Id = CostoDiarioId.New(), Fecha = fecha, IdCentro = idCentro, TipoCosto = tipoCosto.Trim(), Valor = valor, Descripcion = descripcion };
    }
}
