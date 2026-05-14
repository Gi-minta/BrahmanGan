namespace BrahmanGan.Domain.Common;

public sealed class CostoDiarioId : IntId       { private CostoDiarioId(int v) : base(v) { } public static CostoDiarioId From(int v) => new(v); public static CostoDiarioId New() => new(0); }
