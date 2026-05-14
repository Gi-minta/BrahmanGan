namespace BrahmanGan.Domain.Common;

public sealed class AcumulacionInsumoPotreroId : IntId { private AcumulacionInsumoPotreroId(int v) : base(v) { } public static AcumulacionInsumoPotreroId From(int v) => new(v); public static AcumulacionInsumoPotreroId New() => new(0); }
