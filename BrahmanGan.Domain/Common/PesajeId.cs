namespace BrahmanGan.Domain.Common;

public sealed class PesajeId : IntId          { private PesajeId(int v) : base(v) { } public static PesajeId From(int v) => new(v); public static PesajeId New() => new(0); }
