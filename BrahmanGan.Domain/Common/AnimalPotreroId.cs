namespace BrahmanGan.Domain.Common;

public sealed class AnimalPotreroId : IntId   { private AnimalPotreroId(int v) : base(v) { } public static AnimalPotreroId From(int v) => new(v); public static AnimalPotreroId New() => new(0); }
