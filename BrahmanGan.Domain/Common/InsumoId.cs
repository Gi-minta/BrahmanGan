namespace BrahmanGan.Domain.Common;

public sealed class InsumoId : IntId            { private InsumoId(int v) : base(v) { } public static InsumoId From(int v) => new(v); public static InsumoId New() => new(0); }
