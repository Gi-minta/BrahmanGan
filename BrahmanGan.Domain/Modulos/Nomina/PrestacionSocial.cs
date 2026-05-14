using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Nomina;

/// <summary>Prestaciones sociales mensuales del trabajador (Colombia).</summary>
public sealed class PrestacionSocial : Entity<PrestacionSocialId>
{
    public TrabajadorId IdTrabajador { get; private set; } = null!;
    public int Anio { get; private set; }
    public int Mes { get; private set; }
    public decimal SalarioBase { get; private set; }
    public decimal? Cesantias { get; private set; }
    public decimal? Vacaciones { get; private set; }
    public decimal? PrimaServicio { get; private set; }
    public decimal? SaludEmpleador { get; private set; }
    public decimal? PensionEmpleador { get; private set; }
    public decimal? ARL { get; private set; }
    public decimal? CajaCompensacion { get; private set; }
    public decimal? SENA { get; private set; }
    public decimal? ICBF { get; private set; }

    private PrestacionSocial() { }
    public static PrestacionSocial Liquidar(TrabajadorId idTrabajador, int anio, int mes, decimal salarioBase,
        decimal? cesantias = null, decimal? vacaciones = null, decimal? primaServicio = null,
        decimal? saludEmp = null, decimal? pensionEmp = null, decimal? arl = null,
        decimal? caja = null, decimal? sena = null, decimal? icbf = null)
    {
        if (mes is < 1 or > 12) throw new DomainException("Mes debe estar en 1..12");
        if (anio < 1900) throw new DomainException("Año inválido");
        if (salarioBase < 0) throw new DomainException("Salario base no negativo");
        return new PrestacionSocial { Id = PrestacionSocialId.New(), IdTrabajador = idTrabajador,
            Anio = anio, Mes = mes, SalarioBase = salarioBase,
            Cesantias = cesantias, Vacaciones = vacaciones, PrimaServicio = primaServicio,
            SaludEmpleador = saludEmp, PensionEmpleador = pensionEmp, ARL = arl,
            CajaCompensacion = caja, SENA = sena, ICBF = icbf };
    }
}
