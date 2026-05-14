namespace BrahmanGan.Domain.Common;

public sealed class KardexInsumoId : IntId      { private KardexInsumoId(int v) : base(v) { } public static KardexInsumoId From(int v) => new(v); public static KardexInsumoId New() => new(0); }
