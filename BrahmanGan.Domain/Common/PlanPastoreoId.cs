namespace BrahmanGan.Domain.Common;

public sealed class PlanPastoreoId : IntId                { private PlanPastoreoId(int v) : base(v) { } public static PlanPastoreoId From(int v) => new(v); public static PlanPastoreoId New() => new(0); }
