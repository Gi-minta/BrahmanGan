namespace BrahmanGan.Domain.Common;

public sealed class MovimientoAnimalId : IntId{ private MovimientoAnimalId(int v) : base(v) { } public static MovimientoAnimalId From(int v) => new(v); public static MovimientoAnimalId New() => new(0); }
