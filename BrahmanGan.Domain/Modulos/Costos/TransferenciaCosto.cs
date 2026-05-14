using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Transferencia de costo entre centros, sujeta a aprobación.</summary>
public sealed class TransferenciaCosto : Entity<TransferenciaCostoId>
{
    public DateOnly Fecha { get; private set; }
    public CentroCostoId IdCentroOrigen { get; private set; } = null!;
    public CentroCostoId IdCentroDestino { get; private set; } = null!;
    public string Concepto { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public bool Aprobado { get; private set; }

    private TransferenciaCosto() { }
    public static TransferenciaCosto Crear(DateOnly fecha, CentroCostoId origen, CentroCostoId destino, string concepto, decimal valor)
    {
        if (origen.Value == destino.Value) throw new BusinessRuleException("Centro origen y destino deben ser distintos");
        if (string.IsNullOrWhiteSpace(concepto)) throw new DomainException("Concepto requerido");
        if (valor <= 0) throw new DomainException("Valor debe ser > 0");
        return new TransferenciaCosto { Id = TransferenciaCostoId.New(), Fecha = fecha, IdCentroOrigen = origen, IdCentroDestino = destino,
            Concepto = concepto.Trim(), Valor = valor, Aprobado = false };
    }

    public void Aprobar()
    {
        if (Aprobado) throw new BusinessRuleException("La transferencia ya está aprobada");
        Aprobado = true;
        MarkAsModified();
    }
}
