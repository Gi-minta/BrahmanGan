namespace BrahmanGan.Domain.Common;

public sealed class PlanAlimentacionId : IntId            { private PlanAlimentacionId(int v) : base(v) { } public static PlanAlimentacionId From(int v) => new(v); public static PlanAlimentacionId New() => new(0); }
