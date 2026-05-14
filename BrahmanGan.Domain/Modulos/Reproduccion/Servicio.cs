using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Reproduccion;

/// <summary>
/// Servicio reproductivo (monta natural o inseminación artificial).
///
/// Reglas:
///  - Si TipoServicio = IA  → IdSemen es obligatorio, IdToro debe ser null.
///  - Si TipoServicio = MONTA → IdToro es obligatorio, IdSemen debe ser null.
///  - La hembra inseminada se identifica por IdHembra.
///  - El resultado de preñez (true/false) se confirma posteriormente.
/// </summary>
public sealed class Servicio : AggregateRoot<ServicioId>
{
    public AnimalId IdHembra { get; private set; } = null!;
    public TipoServicio TipoServicio { get; private set; }
    public DateOnly Fecha { get; private set; }
    public AnimalId? IdToro { get; private set; }
    public SemenId? IdSemen { get; private set; }
    public bool? ResultadoPreniez { get; private set; }
    public DateOnly? FechaConfirmacion { get; private set; }
    public string? Responsable { get; private set; }

    private Servicio() { }

    public static Servicio RegistrarMonta(AnimalId idHembra, AnimalId idToro, DateOnly fecha, string? responsable = null)
    {
        if (idHembra is null) throw new DomainException("Hembra requerida");
        if (idToro is null) throw new DomainException("Toro requerido para una monta");
        if (idHembra.Value == idToro.Value) throw new BusinessRuleException("Hembra y toro no pueden ser el mismo animal");
        var s = new Servicio
        {
            Id = ServicioId.New(),
            IdHembra = idHembra,
            TipoServicio = TipoServicio.MONTA,
            Fecha = fecha,
            IdToro = idToro,
            Responsable = responsable
        };
        s.AddDomainEvent(new ServicioRegistradoEvent(s.Id, s.IdHembra, s.TipoServicio, s.Fecha));
        return s;
    }

    public static Servicio RegistrarIA(AnimalId idHembra, SemenId idSemen, DateOnly fecha, string? responsable = null)
    {
        if (idHembra is null) throw new DomainException("Hembra requerida");
        if (idSemen is null) throw new DomainException("Semen requerido para una IA");
        var s = new Servicio
        {
            Id = ServicioId.New(),
            IdHembra = idHembra,
            TipoServicio = TipoServicio.IA,
            Fecha = fecha,
            IdSemen = idSemen,
            Responsable = responsable
        };
        s.AddDomainEvent(new ServicioRegistradoEvent(s.Id, s.IdHembra, s.TipoServicio, s.Fecha));
        return s;
    }

    public void ConfirmarResultado(bool preñada, DateOnly fechaConfirmacion)
    {
        if (fechaConfirmacion < Fecha)
            throw new BusinessRuleException("La fecha de confirmación no puede ser anterior al servicio");
        ResultadoPreniez = preñada;
        FechaConfirmacion = fechaConfirmacion;
        AddDomainEvent(new PreniezConfirmadaEvent(Id, IdHembra, preñada));
        IncrementVersion();
    }
}
