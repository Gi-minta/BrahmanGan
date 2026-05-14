namespace BrahmanGan.Domain.Common;

public sealed class CalidadLecheId : IntId        { private CalidadLecheId(int v) : base(v) { } public static CalidadLecheId From(int v) => new(v); public static CalidadLecheId New() => new(0); }
