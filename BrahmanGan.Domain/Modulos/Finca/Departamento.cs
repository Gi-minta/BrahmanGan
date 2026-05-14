using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>Departamento / Estado / Provincia, perteneciente a un país.</summary>
public sealed class Departamento : Entity<DepartamentoId>
{
    public PaisId IdPais { get; private set; } = null!;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;

    private Departamento() { }

    public static Departamento Crear(PaisId idPais, string codigo, string nombre)
    {
        if (idPais is null) throw new DomainException("País requerido");
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        return new Departamento { Id = DepartamentoId.New(), IdPais = idPais, Codigo = codigo.Trim(), Nombre = nombre.Trim() };
    }
}
