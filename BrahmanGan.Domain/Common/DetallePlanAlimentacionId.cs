namespace BrahmanGan.Domain.Common;

public sealed class DetallePlanAlimentacionId : IntId     { private DetallePlanAlimentacionId(int v) : base(v) { } public static DetallePlanAlimentacionId From(int v) => new(v); public static DetallePlanAlimentacionId New() => new(0); }
