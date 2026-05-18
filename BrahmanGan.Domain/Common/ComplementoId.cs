namespace BrahmanGan.Domain.Common;

public sealed class ComplementoId : IntId { private ComplementoId(int v) : base(v) { } public static ComplementoId From(int v) => new(v); public static ComplementoId New() => new(0); }
