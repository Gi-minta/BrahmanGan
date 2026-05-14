using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Activo fijo (vehículos, equipos, instalaciones) asociado a un centro de costo.</summary>
public sealed class Activo : Entity<ActivoId>
{
    public CentroCostoId IdCentro { get; private set; } = null!;
    public string Descripcion { get; private set; } = string.Empty;
    public DateOnly? FechaCompra { get; private set; }
    public decimal? ValorCompra { get; private set; }
    public int? VidaUtilAnios { get; private set; }
    public decimal? ValorResidual { get; private set; }
    public bool EstaActivo { get; private set; } = true;

    private Activo() { }
    public static Activo Crear(CentroCostoId idCentro, string descripcion, DateOnly? fechaCompra = null,
        decimal? valorCompra = null, int? vidaUtilAnios = null, decimal? valorResidual = null)
    {
        if (idCentro is null) throw new DomainException("Centro requerido");
        if (string.IsNullOrWhiteSpace(descripcion)) throw new DomainException("Descripción requerida");
        if (valorCompra is < 0 || valorResidual is < 0) throw new DomainException("Valores no negativos");
        if (vidaUtilAnios is < 0) throw new DomainException("Vida útil no negativa");
        return new Activo { Id = ActivoId.New(), IdCentro = idCentro, Descripcion = descripcion.Trim(), FechaCompra = fechaCompra,
            ValorCompra = valorCompra, VidaUtilAnios = vidaUtilAnios, ValorResidual = valorResidual, EstaActivo = true };
    }
}
