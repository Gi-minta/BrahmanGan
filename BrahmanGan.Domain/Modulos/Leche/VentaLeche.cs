using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Leche;

/// <summary>Venta de leche a un cliente. TotalVenta = LitrosVendidos × PrecioLitro (computado en BD).</summary>
public sealed class VentaLeche : Entity<VentaLecheId>
{
    public DateOnly Fecha { get; private set; }
    public ClienteId IdCliente { get; private set; } = null!;
    public ContratoId? IdContrato { get; private set; }
    public decimal LitrosVendidos { get; private set; }
    public decimal PrecioLitro { get; private set; }
    public decimal TotalVenta => LitrosVendidos * PrecioLitro;
    public string? Factura { get; private set; }

    private VentaLeche() { }

    public static VentaLeche Registrar(DateOnly fecha, ClienteId idCliente, decimal litros, decimal precioLitro,
        ContratoId? idContrato = null, string? factura = null)
    {
        if (idCliente is null) throw new DomainException("Cliente requerido");
        if (litros <= 0) throw new DomainException("Litros vendidos debe ser > 0");
        if (precioLitro <= 0) throw new DomainException("Precio por litro debe ser > 0");
        var v = new VentaLeche
        {
            Id = VentaLecheId.New(),
            Fecha = fecha, IdCliente = idCliente, IdContrato = idContrato,
            LitrosVendidos = litros, PrecioLitro = precioLitro, Factura = factura
        };
        v.AddDomainEvent(new VentaLecheRegistradaEvent(v.Id, v.IdCliente, v.TotalVenta));
        return v;
    }
}
