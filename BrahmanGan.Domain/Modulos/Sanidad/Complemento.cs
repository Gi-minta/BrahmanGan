using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Tratamiento o terapia complementaria asociada a un historial curativo.</summary>
public sealed class Complemento : Entity<ComplementoId>
{
    public HistorialCurativoId IdTratamiento { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public string Descripcion { get; private set; } = string.Empty;
    public string? Tipo { get; private set; }
    public decimal? Costo { get; private set; }

    private Complemento() { }

    public static Complemento Registrar(HistorialCurativoId idTratamiento, DateOnly fecha,
        string descripcion, string? tipo = null, decimal? costo = null)
    {
        if (idTratamiento is null) throw new DomainException("Tratamiento requerido");
        if (string.IsNullOrWhiteSpace(descripcion)) throw new DomainException("Descripción requerida");
        if (costo is < 0) throw new DomainException("Costo no puede ser negativo");
        return new Complemento
        {
            Id = ComplementoId.New(),
            IdTratamiento = idTratamiento,
            Fecha = fecha,
            Descripcion = descripcion.Trim(),
            Tipo = tipo,
            Costo = costo
        };
    }
}
