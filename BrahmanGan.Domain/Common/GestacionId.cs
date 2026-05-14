namespace BrahmanGan.Domain.Common;

public sealed class GestacionId : IntId       { private GestacionId(int v) : base(v) { } public static GestacionId From(int v) => new(v); public static GestacionId New() => new(0); }
