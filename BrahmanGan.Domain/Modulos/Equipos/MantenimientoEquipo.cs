using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Equipos;

/// <summary>Mantenimiento aplicado a una maquinaria. CostoTotal = ManoObra + Repuestos.</summary>
public sealed class MantenimientoEquipo : Entity<MantenimientoEquipoId>
{
    public MaquinariaId IdMaquinaria { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public TipoMantenimiento TipoMantenimiento { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public string? Tecnico { get; private set; }
    public string? Proveedor { get; private set; }
    public decimal CostoManoObra { get; private set; }
    public decimal CostoRepuestos { get; private set; }
    public decimal CostoTotal => CostoManoObra + CostoRepuestos;
    public DateOnly? ProximoMantenimiento { get; private set; }
    public decimal? HorasAlMomento { get; private set; }

    private MantenimientoEquipo() { }
    public static MantenimientoEquipo Registrar(MaquinariaId idMaq, DateOnly fecha, TipoMantenimiento tipo,
        string descripcion, decimal costoManoObra = 0, decimal costoRepuestos = 0,
        string? tecnico = null, string? proveedor = null,
        DateOnly? proximoMantenimiento = null, decimal? horasAlMomento = null)
    {
        if (string.IsNullOrWhiteSpace(descripcion)) throw new DomainException("Descripción requerida");
        if (costoManoObra < 0 || costoRepuestos < 0) throw new DomainException("Costos no negativos");
        if (proximoMantenimiento.HasValue && proximoMantenimiento.Value < fecha)
            throw new BusinessRuleException("Próximo mantenimiento anterior a la fecha");
        return new MantenimientoEquipo { Id = MantenimientoEquipoId.New(), IdMaquinaria = idMaq,
            Fecha = fecha, TipoMantenimiento = tipo, Descripcion = descripcion.Trim(),
            CostoManoObra = costoManoObra, CostoRepuestos = costoRepuestos,
            Tecnico = tecnico, Proveedor = proveedor,
            ProximoMantenimiento = proximoMantenimiento, HorasAlMomento = horasAlMomento };
    }
}
