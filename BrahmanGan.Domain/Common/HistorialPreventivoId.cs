namespace BrahmanGan.Domain.Common;

public sealed class HistorialPreventivoId : IntId       { private HistorialPreventivoId(int v) : base(v) { } public static HistorialPreventivoId From(int v) => new(v); public static HistorialPreventivoId New() => new(0); }
