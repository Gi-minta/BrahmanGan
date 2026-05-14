using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sostenibilidad;

/// <summary>
/// Consumo de agua. LitrosAnimalDia = (VolumenM3 × 1000) / NumAnimales.
/// </summary>
public sealed class ConsumoAgua : Entity<ConsumoAguaId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public PotreroId? IdPotrero { get; private set; }
    public DateOnly Fecha { get; private set; }
    public string? FuenteAgua { get; private set; }
    public decimal VolumenM3 { get; private set; }
    public int? NumAnimales { get; private set; }
    public decimal? LitrosAnimalDia => NumAnimales is > 0 ? (VolumenM3 * 1000m) / NumAnimales.Value : null;
    public string? Observaciones { get; private set; }

    private ConsumoAgua() { }
    public static ConsumoAgua Registrar(FincaId idFinca, DateOnly fecha, decimal volumenM3,
        PotreroId? idPotrero = null, string? fuenteAgua = null, int? numAnimales = null, string? observaciones = null)
    {
        if (volumenM3 < 0) throw new DomainException("Volumen no negativo");
        if (numAnimales is < 0) throw new DomainException("Número de animales no negativo");
        return new ConsumoAgua { Id = ConsumoAguaId.New(), IdFinca = idFinca, IdPotrero = idPotrero,
            Fecha = fecha, FuenteAgua = fuenteAgua, VolumenM3 = volumenM3,
            NumAnimales = numAnimales, Observaciones = observaciones };
    }
}
