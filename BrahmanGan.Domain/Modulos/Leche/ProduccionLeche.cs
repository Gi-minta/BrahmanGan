using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Leche;

/// <summary>
/// Producción agregada de leche por finca y fecha. Único por (IdFinca, Fecha).
///
/// Reglas:
///  - TotalLitros >= LitrosVendidos + LitrosAutoconsumo + LitrosMerma.
///  - Cualquier desglose negativo es inválido.
/// </summary>
public sealed class ProduccionLeche : Entity<ProduccionLecheId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal TotalLitros { get; private set; }
    public decimal? LitrosVendidos { get; private set; }
    public decimal? LitrosAutoconsumo { get; private set; }
    public decimal? LitrosMerma { get; private set; }

    private ProduccionLeche() { }

    public static ProduccionLeche Registrar(FincaId idFinca, DateOnly fecha, decimal totalLitros,
        decimal? vendidos = null, decimal? autoconsumo = null, decimal? merma = null)
    {
        if (idFinca is null) throw new DomainException("Finca requerida");
        if (totalLitros < 0) throw new DomainException("Total no puede ser negativo");
        if (vendidos is < 0 || autoconsumo is < 0 || merma is < 0)
            throw new DomainException("Los desgloses no pueden ser negativos");
        if ((vendidos ?? 0) + (autoconsumo ?? 0) + (merma ?? 0) > totalLitros)
            throw new BusinessRuleException("La suma de desgloses no puede exceder el total");
        var p = new ProduccionLeche
        {
            Id = ProduccionLecheId.New(),
            IdFinca = idFinca, Fecha = fecha,
            TotalLitros = totalLitros, LitrosVendidos = vendidos,
            LitrosAutoconsumo = autoconsumo, LitrosMerma = merma
        };
        p.AddDomainEvent(new ProduccionLecheRegistradaEvent(p.Id, p.IdFinca, p.Fecha, p.TotalLitros));
        return p;
    }
}
