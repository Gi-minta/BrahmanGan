namespace BrahmanGan.Domain.Common;

public sealed class EventoMedioambientalId : IntId { private EventoMedioambientalId(int v) : base(v) { } public static EventoMedioambientalId From(int v) => new(v); public static EventoMedioambientalId New() => new(0); }
