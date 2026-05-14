using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sostenibilidad;

/// <summary>Evento ambiental relevante (sequía, inundación, plaga, ola de calor).</summary>
public sealed class EventoMedioambiental : Entity<EventoMedioambientalId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public string TipoEvento { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public string? Intensidad { get; private set; }
    public decimal? PrecipitacionMM { get; private set; }
    public decimal? TempMaxC { get; private set; }
    public decimal? TempMinC { get; private set; }
    public string? ImpactoEstimado { get; private set; }

    private EventoMedioambiental() { }
    public static EventoMedioambiental Registrar(FincaId idFinca, DateOnly fecha, string tipoEvento,
        string? descripcion = null, string? intensidad = null,
        decimal? precipitacionMM = null, decimal? tempMaxC = null, decimal? tempMinC = null,
        string? impactoEstimado = null)
    {
        if (string.IsNullOrWhiteSpace(tipoEvento)) throw new DomainException("Tipo de evento requerido");
        if (precipitacionMM is < 0) throw new DomainException("Precipitación no negativa");
        if (tempMaxC.HasValue && tempMinC.HasValue && tempMaxC < tempMinC)
            throw new BusinessRuleException("TempMax < TempMin");
        return new EventoMedioambiental { Id = EventoMedioambientalId.New(), IdFinca = idFinca, Fecha = fecha,
            TipoEvento = tipoEvento.Trim(), Descripcion = descripcion, Intensidad = intensidad,
            PrecipitacionMM = precipitacionMM, TempMaxC = tempMaxC, TempMinC = tempMinC,
            ImpactoEstimado = impactoEstimado };
    }
}
