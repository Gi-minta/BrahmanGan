namespace BrahmanGan.Domain.Common;

public sealed class DetalleCurativoId : IntId           { private DetalleCurativoId(int v) : base(v) { } public static DetalleCurativoId From(int v) => new(v); public static DetalleCurativoId New() => new(0); }
