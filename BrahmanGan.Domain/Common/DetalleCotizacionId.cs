namespace BrahmanGan.Domain.Common;

public sealed class DetalleCotizacionId : IntId { private DetalleCotizacionId(int v) : base(v) { } public static DetalleCotizacionId From(int v) => new(v); public static DetalleCotizacionId New() => new(0); }
