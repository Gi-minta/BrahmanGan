namespace BrahmanGan.Domain.Common;

public sealed class VentaLecheId : IntId          { private VentaLecheId(int v) : base(v) { } public static VentaLecheId From(int v) => new(v); public static VentaLecheId New() => new(0); }
