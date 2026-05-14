namespace BrahmanGan.Domain.Common;

public sealed class HistorialAnimalId : IntId { private HistorialAnimalId(int v) : base(v) { } public static HistorialAnimalId From(int v) => new(v); public static HistorialAnimalId New() => new(0); }
