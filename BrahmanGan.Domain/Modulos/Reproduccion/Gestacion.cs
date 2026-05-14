using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Reproduccion;

/// <summary>
/// Gestación de una hembra. Se inicia tras confirmar preñez en un servicio.
///
/// Reglas:
///  - Estado por defecto: EN_CURSO.
///  - Solo gestaciones EN_CURSO pueden registrar parto o aborto.
///  - Período de gestación bovino aproximado: 283 días (sirve para FechaPartoEstimado).
/// </summary>
public sealed class Gestacion : AggregateRoot<GestacionId>
{
    public const int DiasGestacionBovino = 283;

    public AnimalId IdAnimal { get; private set; } = null!;
    public ServicioId? IdServicio { get; private set; }
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaPartoEstimado { get; private set; }
    public DateOnly? FechaPartoReal { get; private set; }
    public EstadoGestacion Estado { get; private set; } = EstadoGestacion.EN_CURSO;
    public string? Observaciones { get; private set; }

    private Gestacion() { }

    public static Gestacion Iniciar(AnimalId idAnimal, DateOnly fechaInicio, ServicioId? idServicio = null, string? observaciones = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        var g = new Gestacion
        {
            Id = GestacionId.New(),
            IdAnimal = idAnimal,
            IdServicio = idServicio,
            FechaInicio = fechaInicio,
            FechaPartoEstimado = fechaInicio.AddDays(DiasGestacionBovino),
            Estado = EstadoGestacion.EN_CURSO,
            Observaciones = observaciones
        };
        g.AddDomainEvent(new GestacionIniciadaEvent(g.Id, g.IdAnimal, g.FechaPartoEstimado!.Value));
        return g;
    }

    public void RegistrarParto(DateOnly fechaPartoReal)
    {
        if (Estado != EstadoGestacion.EN_CURSO)
            throw new BusinessRuleException($"No se puede registrar parto en gestación {Estado}");
        if (fechaPartoReal < FechaInicio)
            throw new BusinessRuleException("Fecha de parto anterior al inicio de gestación");
        FechaPartoReal = fechaPartoReal;
        Estado = EstadoGestacion.PARTO;
        AddDomainEvent(new PartoRegistradoEvent(Id, IdAnimal, fechaPartoReal));
        IncrementVersion();
    }

    public void RegistrarAborto(DateOnly fecha, string? motivo = null)
    {
        if (Estado != EstadoGestacion.EN_CURSO)
            throw new BusinessRuleException($"No se puede registrar aborto en gestación {Estado}");
        FechaPartoReal = fecha;
        Estado = EstadoGestacion.ABORTO;
        if (!string.IsNullOrWhiteSpace(motivo))
            Observaciones = $"{Observaciones}\nABORTO: {motivo}".Trim();
        AddDomainEvent(new AbortoRegistradoEvent(Id, IdAnimal, fecha));
        IncrementVersion();
    }

    public void Interrumpir(string motivo)
    {
        if (Estado != EstadoGestacion.EN_CURSO)
            throw new BusinessRuleException("Solo se interrumpen gestaciones EN_CURSO");
        if (string.IsNullOrWhiteSpace(motivo)) throw new DomainException("Motivo requerido");
        Estado = EstadoGestacion.INTERRUMPIDA;
        Observaciones = $"{Observaciones}\nINTERRUMPIDA: {motivo}".Trim();
        IncrementVersion();
    }
}
