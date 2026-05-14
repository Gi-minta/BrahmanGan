using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>Municipio normalizado, perteneciente a un departamento. Reemplaza textos libres.</summary>
public sealed class Municipio : Entity<MunicipioId>
{
    public DepartamentoId IdDepto { get; private set; } = null!;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;

    private Municipio() { }

    public static Municipio Crear(DepartamentoId idDepto, string codigo, string nombre)
    {
        if (idDepto is null) throw new DomainException("Departamento requerido");
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        return new Municipio { Id = MunicipioId.New(), IdDepto = idDepto, Codigo = codigo.Trim(), Nombre = nombre.Trim() };
    }
}
