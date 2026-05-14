using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>
/// Origen del animal (NACIDO_FINCA, COMPRADO, DONADO, etc.).
/// </summary>
public sealed class Origen : Entity<OrigenId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;

    private Origen() { }

    public static Origen Crear(string codigo, string descripcion)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de origen requerido");
        if (string.IsNullOrWhiteSpace(descripcion)) throw new DomainException("Descripción de origen requerida");
        return new Origen { Id = OrigenId.New(), Codigo = codigo.Trim().ToUpperInvariant(), Descripcion = descripcion.Trim() };
    }
}
