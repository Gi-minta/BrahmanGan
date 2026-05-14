using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>
/// Zona OPERATIVA / logística (no geográfica): rutas de recolección, sectores veterinarios, etc.
/// Una finca puede pertenecer a varias zonas (relación N:M vía ZonaFinca).
/// </summary>
public sealed class Zona : Entity<ZonaId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Tipo { get; private set; }
    public string? Descripcion { get; private set; }
    public bool Activa { get; private set; } = true;

    private Zona() { }

    public static Zona Crear(string codigo, string nombre, string? tipo = null, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de zona requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de zona requerido");
        return new Zona { Id = ZonaId.New(), Codigo = codigo.Trim().ToUpperInvariant(), Nombre = nombre.Trim(), Tipo = tipo, Descripcion = descripcion, Activa = true };
    }

    public void Activar() { Activa = true; MarkAsModified(); }
    public void Desactivar() { Activa = false; MarkAsModified(); }
}
