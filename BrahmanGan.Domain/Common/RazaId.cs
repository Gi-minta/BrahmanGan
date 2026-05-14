namespace BrahmanGan.Domain.Common;

public sealed class RazaId : IntId            { private RazaId(int v) : base(v) { } public static RazaId From(int v) => new(v); public static RazaId New() => new(0); }
