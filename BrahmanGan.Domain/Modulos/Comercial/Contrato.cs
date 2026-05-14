using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Comercial;

/// <summary>Contrato comercial con un cliente (suministro de leche, animales, etc.).</summary>
public sealed class Contrato : Entity<ContratoId>
{
    public ClienteId IdCliente { get; private set; } = null!;
    public string Tipo { get; private set; } = string.Empty;
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaFin { get; private set; }
    public decimal? PrecioAcordado { get; private set; }
    public string? UnidadPrecio { get; private set; }
    public decimal? VolumenEstimado { get; private set; }
    public string? Condiciones { get; private set; }
    public string Estado { get; private set; } = "VIGENTE";

    private Contrato() { }

    public static Contrato Crear(ClienteId idCliente, string tipo, DateOnly fechaInicio,
        DateOnly? fechaFin = null, decimal? precioAcordado = null, string? unidadPrecio = null,
        decimal? volumenEstimado = null, string? condiciones = null)
    {
        if (idCliente is null) throw new DomainException("Cliente requerido");
        if (string.IsNullOrWhiteSpace(tipo)) throw new DomainException("Tipo de contrato requerido");
        if (fechaFin.HasValue && fechaFin.Value < fechaInicio)
            throw new BusinessRuleException("FechaFin < FechaInicio");
        if (precioAcordado is < 0) throw new DomainException("Precio no negativo");
        return new Contrato
        {
            Id = ContratoId.New(),
            IdCliente = idCliente, Tipo = tipo.Trim(), FechaInicio = fechaInicio,
            FechaFin = fechaFin, PrecioAcordado = precioAcordado, UnidadPrecio = unidadPrecio,
            VolumenEstimado = volumenEstimado, Condiciones = condiciones, Estado = "VIGENTE"
        };
    }

    public void Cerrar(DateOnly fechaFin)
    {
        if (fechaFin < FechaInicio) throw new BusinessRuleException("FechaFin < FechaInicio");
        FechaFin = fechaFin;
        Estado = "CERRADO";
        MarkAsModified();
    }

    public void Cancelar()
    {
        if (Estado != "VIGENTE") throw new BusinessRuleException("Solo contratos VIGENTES se cancelan");
        Estado = "CANCELADO";
        MarkAsModified();
    }
}
