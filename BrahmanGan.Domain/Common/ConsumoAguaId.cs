namespace BrahmanGan.Domain.Common;

public sealed class ConsumoAguaId : IntId       { private ConsumoAguaId(int v) : base(v) { } public static ConsumoAguaId From(int v) => new(v); public static ConsumoAguaId New() => new(0); }
