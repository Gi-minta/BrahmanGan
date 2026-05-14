using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Almacen;

/// <summary>
/// Insumo de almacén (forrajes, medicamentos genéricos, repuestos, etc.).
/// Stock se actualiza vía Kardex; el valor se persiste denormalizado para consultas rápidas.
/// </summary>
public sealed class Insumo : AggregateRoot<InsumoId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Tipo { get; private set; }
    public string? UnidadMedida { get; private set; }
    public decimal? PrecioUnitario { get; private set; }
    public decimal StockMinimo { get; private set; }
    public decimal StockActual { get; private set; }
    public bool Activo { get; private set; } = true;

    private Insumo() { }
    public static Insumo Crear(string codigo, string nombre, string? tipo = null, string? unidadMedida = null,
        decimal? precioUnitario = null, decimal stockMinimo = 0, decimal stockInicial = 0)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        if (precioUnitario is < 0) throw new DomainException("Precio no negativo");
        if (stockMinimo < 0 || stockInicial < 0) throw new DomainException("Stock no negativo");
        return new Insumo { Id = InsumoId.New(), Codigo = codigo.Trim(), Nombre = nombre.Trim(),
            Tipo = tipo, UnidadMedida = unidadMedida, PrecioUnitario = precioUnitario,
            StockMinimo = stockMinimo, StockActual = stockInicial, Activo = true };
    }

    /// <summary>Aplica un movimiento de Kardex y actualiza el stock. Devuelve saldos para registrarse en el Kardex.</summary>
    public (decimal saldoAnterior, decimal saldoNuevo) AplicarMovimiento(TipoMovimientoKardex tipo, decimal cantidad)
    {
        if (cantidad <= 0) throw new BusinessRuleException("La cantidad debe ser > 0");
        var saldoAnterior = StockActual;
        var saldoNuevo = tipo switch
        {
            TipoMovimientoKardex.ENTRADA => saldoAnterior + cantidad,
            TipoMovimientoKardex.SALIDA  => saldoAnterior - cantidad,
            TipoMovimientoKardex.AJUSTE  => cantidad,
            _ => throw new DomainException("Tipo de movimiento desconocido")
        };
        if (saldoNuevo < 0) throw new BusinessRuleException("Stock insuficiente");
        StockActual = saldoNuevo;
        IncrementVersion();
        return (saldoAnterior, saldoNuevo);
    }

    public bool BajoMinimo() => StockActual < StockMinimo;
}
