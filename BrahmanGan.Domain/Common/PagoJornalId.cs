namespace BrahmanGan.Domain.Common;

public sealed class PagoJornalId : IntId        { private PagoJornalId(int v) : base(v) { } public static PagoJornalId From(int v) => new(v); public static PagoJornalId New() => new(0); }
