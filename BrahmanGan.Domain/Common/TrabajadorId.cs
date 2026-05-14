namespace BrahmanGan.Domain.Common;

public sealed class TrabajadorId : IntId        { private TrabajadorId(int v) : base(v) { } public static TrabajadorId From(int v) => new(v); public static TrabajadorId New() => new(0); }
