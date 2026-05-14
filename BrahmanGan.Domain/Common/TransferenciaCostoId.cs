namespace BrahmanGan.Domain.Common;

public sealed class TransferenciaCostoId : IntId{ private TransferenciaCostoId(int v) : base(v) { } public static TransferenciaCostoId From(int v) => new(v); public static TransferenciaCostoId New() => new(0); }
