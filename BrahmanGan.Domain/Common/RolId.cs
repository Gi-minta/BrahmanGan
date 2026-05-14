namespace BrahmanGan.Domain.Common;

public sealed class RolId : IntId       { private RolId(int v) : base(v) { } public static RolId From(int v) => new(v); public static RolId New() => new(0); }
