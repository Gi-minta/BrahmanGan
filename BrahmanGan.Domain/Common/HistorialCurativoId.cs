namespace BrahmanGan.Domain.Common;

public sealed class HistorialCurativoId : IntId         { private HistorialCurativoId(int v) : base(v) { } public static HistorialCurativoId From(int v) => new(v); public static HistorialCurativoId New() => new(0); }
