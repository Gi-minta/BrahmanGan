using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>Tabla puente Zona ↔ Finca (relación N:M con vigencia temporal).</summary>
public sealed class ZonaFinca : Entity<ZonaFincaId>
{
    public ZonaId IdZona { get; private set; } = null!;
    public FincaId IdFinca { get; private set; } = null!;
    public DateOnly FechaIngreso { get; private set; }
    public DateOnly? FechaSalida { get; private set; }
    public string? Observaciones { get; private set; }

    private ZonaFinca() { }

    public static ZonaFinca Crear(ZonaId idZona, FincaId idFinca, DateOnly? fechaIngreso = null, string? observaciones = null)
    {
        if (idZona is null) throw new DomainException("Zona requerida");
        if (idFinca is null) throw new DomainException("Finca requerida");
        return new ZonaFinca
        {
            Id = ZonaFincaId.New(),
            IdZona = idZona,
            IdFinca = idFinca,
            FechaIngreso = fechaIngreso ?? DateOnly.FromDateTime(DateTime.UtcNow),
            Observaciones = observaciones
        };
    }

    public void Cerrar(DateOnly fechaSalida)
    {
        if (fechaSalida < FechaIngreso) throw new BusinessRuleException("Fecha de salida no puede ser anterior al ingreso");
        FechaSalida = fechaSalida;
        MarkAsModified();
    }
}
