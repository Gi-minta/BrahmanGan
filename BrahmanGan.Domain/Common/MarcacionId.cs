namespace BrahmanGan.Domain.Common;

public sealed class MarcacionId : IntId       { private MarcacionId(int v) : base(v) { } public static MarcacionId From(int v) => new(v); public static MarcacionId New() => new(0); }
