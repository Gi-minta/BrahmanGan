namespace BrahmanGan.Domain.Common;

public sealed class GastoGeneralId : IntId      { private GastoGeneralId(int v) : base(v) { } public static GastoGeneralId From(int v) => new(v); public static GastoGeneralId New() => new(0); }
