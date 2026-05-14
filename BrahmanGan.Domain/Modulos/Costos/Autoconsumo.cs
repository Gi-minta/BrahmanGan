using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Autoconsumo (mercancía consumida internamente).</summary>
public sealed class Autoconsumo : Entity<AutoconsumoId>
{
    public DateOnly Fecha { get; private set; }
    public CentroCostoId IdCentro { get; private set; } = null!;
    public string Concepto { get; private set; } = string.Empty;
    public decimal? Cantidad { get; private set; }
    public decimal? ValorUnitario { get; private set; }
    public decimal ValorTotal { get; private set; }

    private Autoconsumo() { }
    public static Autoconsumo Crear(DateOnly fecha, CentroCostoId idCentro, string concepto, decimal valorTotal,
        decimal? cantidad = null, decimal? valorUnitario = null)
    {
        if (string.IsNullOrWhiteSpace(concepto)) throw new DomainException("Concepto requerido");
        if (valorTotal < 0) throw new DomainException("Valor total no negativo");
        return new Autoconsumo { Id = AutoconsumoId.New(), Fecha = fecha, IdCentro = idCentro, Concepto = concepto.Trim(),
            Cantidad = cantidad, ValorUnitario = valorUnitario, ValorTotal = valorTotal };
    }
}
