namespace BrahmanGan.Domain.Common;

public sealed class GrupoManejoId : IntId     { private GrupoManejoId(int v) : base(v) { } public static GrupoManejoId From(int v) => new(v); public static GrupoManejoId New() => new(0); }
