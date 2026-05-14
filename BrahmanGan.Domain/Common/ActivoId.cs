namespace BrahmanGan.Domain.Common;

public sealed class ActivoId : IntId            { private ActivoId(int v) : base(v) { } public static ActivoId From(int v) => new(v); public static ActivoId New() => new(0); }
