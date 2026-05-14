namespace BrahmanGan.Domain.Common;

public sealed class CostoAnimalDiarioId : IntId { private CostoAnimalDiarioId(int v) : base(v) { } public static CostoAnimalDiarioId From(int v) => new(v); public static CostoAnimalDiarioId New() => new(0); }
