namespace BrahmanGan.Domain.Common;

public sealed class NacimientoId : IntId      { private NacimientoId(int v) : base(v) { } public static NacimientoId From(int v) => new(v); public static NacimientoId New() => new(0); }
