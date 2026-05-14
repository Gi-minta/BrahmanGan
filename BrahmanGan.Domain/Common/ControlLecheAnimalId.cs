namespace BrahmanGan.Domain.Common;

public sealed class ControlLecheAnimalId : IntId  { private ControlLecheAnimalId(int v) : base(v) { } public static ControlLecheAnimalId From(int v) => new(v); public static ControlLecheAnimalId New() => new(0); }
