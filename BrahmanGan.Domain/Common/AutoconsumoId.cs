namespace BrahmanGan.Domain.Common;

public sealed class AutoconsumoId : IntId       { private AutoconsumoId(int v) : base(v) { } public static AutoconsumoId From(int v) => new(v); public static AutoconsumoId New() => new(0); }
