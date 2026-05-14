namespace BrahmanGan.Domain.Common;

public sealed class PermisoId : IntId   { private PermisoId(int v) : base(v) { } public static PermisoId From(int v) => new(v); public static PermisoId New() => new(0); }
