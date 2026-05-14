namespace BrahmanGan.Domain.Common;

public sealed class PotreroId : IntId         { private PotreroId(int v) : base(v) { } public static PotreroId From(int v) => new(v); public static PotreroId New() => new(0); }
