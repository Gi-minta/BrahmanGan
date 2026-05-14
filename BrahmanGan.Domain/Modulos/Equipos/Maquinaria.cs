using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Equipos;

/// <summary>Maquinaria / vehículos / equipos pesados de la finca.</summary>
public sealed class Maquinaria : AggregateRoot<MaquinariaId>
{
    public CentroCostoId IdCentro { get; private set; } = null!;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Marca { get; private set; }
    public string? Modelo { get; private set; }
    public int? Anio { get; private set; }
    public string? NumeroSerie { get; private set; }
    public DateOnly? FechaCompra { get; private set; }
    public decimal? ValorCompra { get; private set; }
    public decimal HorasUso { get; private set; }
    public EstadoMaquinaria Estado { get; private set; } = EstadoMaquinaria.OPERATIVO;
    public string? Observaciones { get; private set; }

    private Maquinaria() { }
    public static Maquinaria Crear(CentroCostoId idCentro, string codigo, string nombre,
        string? marca = null, string? modelo = null, int? anio = null, string? numeroSerie = null,
        DateOnly? fechaCompra = null, decimal? valorCompra = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        if (valorCompra is < 0) throw new DomainException("Valor no negativo");
        if (anio is < 1900) throw new DomainException("Año inválido");
        return new Maquinaria { Id = MaquinariaId.New(), IdCentro = idCentro, Codigo = codigo.Trim(),
            Nombre = nombre.Trim(), Marca = marca, Modelo = modelo, Anio = anio,
            NumeroSerie = numeroSerie, FechaCompra = fechaCompra, ValorCompra = valorCompra,
            HorasUso = 0, Estado = EstadoMaquinaria.OPERATIVO };
    }

    public void RegistrarHoras(decimal horas)
    {
        if (horas <= 0) throw new BusinessRuleException("Horas debe ser > 0");
        HorasUso += horas;
        IncrementVersion();
    }

    public void CambiarEstado(EstadoMaquinaria nuevo)
    {
        if (Estado == EstadoMaquinaria.BAJA && nuevo != EstadoMaquinaria.BAJA)
            throw new BusinessRuleException("Maquinaria dada de baja no puede cambiar de estado");
        if (Estado == nuevo) return;
        Estado = nuevo;
        IncrementVersion();
    }
}
