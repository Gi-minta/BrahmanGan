namespace BrahmanGan.Domain.Common;

public sealed class ServicioId : IntId        { private ServicioId(int v) : base(v) { } public static ServicioId From(int v) => new(v); public static ServicioId New() => new(0); }
