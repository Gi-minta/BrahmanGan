namespace BrahmanGan.Domain.Common;

public sealed class MaquinariaId : IntId        { private MaquinariaId(int v) : base(v) { } public static MaquinariaId From(int v) => new(v); public static MaquinariaId New() => new(0); }
