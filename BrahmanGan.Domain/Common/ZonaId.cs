namespace BrahmanGan.Domain.Common;

public sealed class ZonaId : IntId            { private ZonaId(int v) : base(v) { } public static ZonaId From(int v) => new(v); public static ZonaId New() => new(0); }
