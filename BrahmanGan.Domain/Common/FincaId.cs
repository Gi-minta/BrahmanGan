namespace BrahmanGan.Domain.Common;

public sealed class FincaId : IntId           { private FincaId(int v) : base(v) { } public static FincaId From(int v) => new(v); public static FincaId New() => new(0); }
