namespace BrahmanGan.Domain.Common;

public sealed class ZonaFincaId : IntId       { private ZonaFincaId(int v) : base(v) { } public static ZonaFincaId From(int v) => new(v); public static ZonaFincaId New() => new(0); }
