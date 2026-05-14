using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Leche;

/// <summary>Análisis de calidad de la leche (sanidad y composición).</summary>
public sealed class CalidadLeche : Entity<CalidadLecheId>
{
    public AnimalId? IdAnimal { get; private set; }
    public DateOnly Fecha { get; private set; }
    public int? CelSomaticas { get; private set; }
    public decimal? GrasaPct { get; private set; }
    public decimal? ProteinaPct { get; private set; }
    public decimal? LactozaPct { get; private set; }
    public decimal? UreaMgDL { get; private set; }
    public string? Laboratorio { get; private set; }
    public string? Resultado { get; private set; }
    public string? Observaciones { get; private set; }

    private CalidadLeche() { }

    public static CalidadLeche Registrar(DateOnly fecha, AnimalId? idAnimal = null,
        int? celSomaticas = null, decimal? grasa = null, decimal? proteina = null,
        decimal? lactoza = null, decimal? urea = null, string? laboratorio = null,
        string? resultado = null, string? observaciones = null)
    {
        if (celSomaticas is < 0) throw new DomainException("Células somáticas no negativas");
        return new CalidadLeche
        {
            Id = CalidadLecheId.New(),
            IdAnimal = idAnimal, Fecha = fecha,
            CelSomaticas = celSomaticas, GrasaPct = grasa, ProteinaPct = proteina,
            LactozaPct = lactoza, UreaMgDL = urea, Laboratorio = laboratorio,
            Resultado = resultado, Observaciones = observaciones
        };
    }
}
