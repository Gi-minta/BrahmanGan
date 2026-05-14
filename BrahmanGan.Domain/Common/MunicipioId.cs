namespace BrahmanGan.Domain.Common;

public sealed class MunicipioId : IntId       { private MunicipioId(int v) : base(v) { } public static MunicipioId From(int v) => new(v); public static MunicipioId New() => new(0); }
