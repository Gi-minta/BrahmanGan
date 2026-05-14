using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Centro de costo de la finca (productivo, administrativo, etc.).</summary>
public sealed class CentroCosto : Entity<CentroCostoId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public FincaId? IdFinca { get; private set; }
    public bool Activo { get; private set; } = true;

    private CentroCosto() { }
    public static CentroCosto Crear(string codigo, string nombre, FincaId? idFinca = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        return new CentroCosto { Id = CentroCostoId.New(), Codigo = codigo.Trim().ToUpperInvariant(), Nombre = nombre.Trim(), IdFinca = idFinca, Activo = true };
    }
}
