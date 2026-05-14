namespace BrahmanGan.Domain.Common;

public sealed class CotizacionVentaId : IntId   { private CotizacionVentaId(int v) : base(v) { } public static CotizacionVentaId From(int v) => new(v); public static CotizacionVentaId New() => new(0); }
