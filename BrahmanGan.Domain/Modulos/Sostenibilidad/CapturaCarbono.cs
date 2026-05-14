using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sostenibilidad;

/// <summary>Captura mensual de carbono por finca. HuellaNeta = EmisionesGanado - CapturaForestal.</summary>
public sealed class CapturaCarbono : Entity<CapturaCarbonoId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public int Anio { get; private set; }
    public int Mes { get; private set; }
    public decimal? EmisionesGanadoTCO2 { get; private set; }
    public decimal? CapturaForestal { get; private set; }
    public decimal HuellaNeta => (EmisionesGanadoTCO2 ?? 0m) - (CapturaForestal ?? 0m);
    public string? Certificacion { get; private set; }
    public string? Observaciones { get; private set; }

    private CapturaCarbono() { }
    public static CapturaCarbono Registrar(FincaId idFinca, int anio, int mes,
        decimal? emisiones = null, decimal? captura = null,
        string? certificacion = null, string? observaciones = null)
    {
        if (mes is < 1 or > 12) throw new DomainException("Mes 1..12");
        if (anio < 1900) throw new DomainException("Año inválido");
        if (emisiones is < 0 || captura is < 0) throw new DomainException("Valores no negativos");
        return new CapturaCarbono { Id = CapturaCarbonoId.New(), IdFinca = idFinca, Anio = anio, Mes = mes,
            EmisionesGanadoTCO2 = emisiones, CapturaForestal = captura,
            Certificacion = certificacion, Observaciones = observaciones };
    }
}
