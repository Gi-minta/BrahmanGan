using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Protocolo o control sanitario preventivo (ej: vacunación periódica, desparasitación programada).</summary>
public sealed class ControlPreventivo : Entity<ControlPreventivoId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public int? FrecuenciaDias { get; private set; }
    public bool Activo { get; private set; } = true;

    private ControlPreventivo() { }

    public static ControlPreventivo Crear(string codigo, string nombre,
        string? descripcion = null, int? frecuenciaDias = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        if (frecuenciaDias is <= 0) throw new DomainException("Frecuencia debe ser > 0");
        return new ControlPreventivo
        {
            Id = ControlPreventivoId.New(),
            Codigo = codigo.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            FrecuenciaDias = frecuenciaDias,
            Activo = true
        };
    }

    public void Desactivar() { Activo = false; MarkAsModified(); }
}
