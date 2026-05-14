namespace BrahmanGan.Domain.Common;

public sealed class DepartamentoId : IntId    { private DepartamentoId(int v) : base(v) { } public static DepartamentoId From(int v) => new(v); public static DepartamentoId New() => new(0); }
