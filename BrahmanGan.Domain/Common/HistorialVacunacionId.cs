namespace BrahmanGan.Domain.Common;

public sealed class HistorialVacunacionId : IntId       { private HistorialVacunacionId(int v) : base(v) { } public static HistorialVacunacionId From(int v) => new(v); public static HistorialVacunacionId New() => new(0); }
