namespace BrahmanGan.Domain.Common;

public sealed class HistorialDesparasitacionId : IntId  { private HistorialDesparasitacionId(int v) : base(v) { } public static HistorialDesparasitacionId From(int v) => new(v); public static HistorialDesparasitacionId New() => new(0); }
