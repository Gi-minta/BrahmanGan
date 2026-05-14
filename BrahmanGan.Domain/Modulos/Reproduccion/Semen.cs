using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Reproduccion;

/// <summary>Inventario de pajillas de semen (para inseminación artificial).</summary>
public sealed class Semen : Entity<SemenId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string NombreToro { get; private set; } = string.Empty;
    public RazaId? IdRaza { get; private set; }
    public string? Casa { get; private set; }
    public int StockDosis { get; private set; }
    public bool Activo { get; private set; } = true;

    private Semen() { }

    public static Semen Crear(string codigo, string nombreToro, RazaId? idRaza = null, string? casa = null, int stockInicial = 0)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de semen requerido");
        if (string.IsNullOrWhiteSpace(nombreToro)) throw new DomainException("Nombre del toro requerido");
        if (stockInicial < 0) throw new DomainException("Stock no puede ser negativo");
        return new Semen
        {
            Id = SemenId.New(),
            Codigo = codigo.Trim(),
            NombreToro = nombreToro.Trim(),
            IdRaza = idRaza,
            Casa = casa,
            StockDosis = stockInicial,
            Activo = true
        };
    }

    public void IngresarStock(int dosis)
    {
        if (dosis <= 0) throw new BusinessRuleException("Dosis a ingresar debe ser > 0");
        StockDosis += dosis;
        MarkAsModified();
    }

    public void ConsumirDosis(int dosis = 1)
    {
        if (dosis <= 0) throw new BusinessRuleException("Dosis a consumir debe ser > 0");
        if (StockDosis < dosis) throw new BusinessRuleException("Stock insuficiente de semen");
        StockDosis -= dosis;
        MarkAsModified();
    }
}
