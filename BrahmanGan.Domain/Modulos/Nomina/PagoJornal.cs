using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Nomina;

/// <summary>Pago por jornal (trabajadores no asalariados).</summary>
public sealed class PagoJornal : Entity<PagoJornalId>
{
    public TrabajadorId IdTrabajador { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal? HorasTrabajadas { get; private set; }
    public decimal ValorJornal { get; private set; }
    public string? Concepto { get; private set; }
    public CentroCostoId? IdCentro { get; private set; }

    private PagoJornal() { }
    public static PagoJornal Registrar(TrabajadorId idTrabajador, DateOnly fecha, decimal valorJornal,
        decimal? horasTrabajadas = null, string? concepto = null, CentroCostoId? idCentro = null)
    {
        if (valorJornal <= 0) throw new DomainException("Valor jornal debe ser > 0");
        if (horasTrabajadas is < 0) throw new DomainException("Horas no negativas");
        return new PagoJornal { Id = PagoJornalId.New(), IdTrabajador = idTrabajador, Fecha = fecha,
            HorasTrabajadas = horasTrabajadas, ValorJornal = valorJornal,
            Concepto = concepto, IdCentro = idCentro };
    }
}
