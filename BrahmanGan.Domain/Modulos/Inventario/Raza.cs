using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>
/// Catálogo de razas bovinas (ej: BRAHMAN, CEBU, HOLSTEIN).
/// </summary>
public sealed class Raza : Entity<RazaId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public PropositoRaza? Proposito { get; private set; }

    private Raza() { }

    public static Raza Crear(string codigo, string nombre, PropositoRaza? proposito = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de raza requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de raza requerido");
        return new Raza
        {
            Id = RazaId.New(),
            Codigo = codigo.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Proposito = proposito
        };
    }

    public void Renombrar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de raza requerido");
        Nombre = nombre.Trim();
        MarkAsModified();
    }
}
