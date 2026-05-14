namespace BrahmanGan.Domain.Common;

public sealed class HistorialMastitisId : IntId         { private HistorialMastitisId(int v) : base(v) { } public static HistorialMastitisId From(int v) => new(v); public static HistorialMastitisId New() => new(0); }
