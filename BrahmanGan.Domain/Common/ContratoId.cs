namespace BrahmanGan.Domain.Common;

public sealed class ContratoId : IntId          { private ContratoId(int v) : base(v) { } public static ContratoId From(int v) => new(v); public static ContratoId New() => new(0); }
