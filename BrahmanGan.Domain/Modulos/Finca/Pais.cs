using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>País (raíz de la jerarquía geográfica). Código ISO 3166-1 alpha-2.</summary>
public sealed class Pais : Entity<PaisId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;

    private Pais() { }

    public static Pais Crear(string codigo, string nombre)
    {
        if (string.IsNullOrWhiteSpace(codigo) || codigo.Length > 5)
            throw new DomainException("Código de país requerido (máx. 5 caracteres)");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de país requerido");
        return new Pais { Id = PaisId.New(), Codigo = codigo.Trim().ToUpperInvariant(), Nombre = nombre.Trim() };
    }
}
