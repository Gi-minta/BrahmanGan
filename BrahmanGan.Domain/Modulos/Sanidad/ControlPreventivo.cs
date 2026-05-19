using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Protocolo o control sanitario preventivo (ej: vacunación periódica, desparasitación programada).</summary>
public sealed class ControlPreventivo : Entity<ControlPreventivoId>
{
    public string Nombre { get; private set; } = string.Empty;
    public string? Periodicidad { get; private set; }
    public string? Descripcion { get; private set; }

    private ControlPreventivo() { }

    public static ControlPreventivo Crear(string nombre, string? periodicidad = null, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        return new ControlPreventivo
        {
            Id = ControlPreventivoId.New(),
            Nombre = nombre.Trim(),
            Periodicidad = periodicidad,
            Descripcion = descripcion
        };
    }
}

