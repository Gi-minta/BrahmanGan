namespace BrahmanGan.Domain.Common;

public sealed class IngresoId : IntId           { private IngresoId(int v) : base(v) { } public static IngresoId From(int v) => new(v); public static IngresoId New() => new(0); }
