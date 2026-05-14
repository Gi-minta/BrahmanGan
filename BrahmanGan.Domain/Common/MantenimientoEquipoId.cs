namespace BrahmanGan.Domain.Common;

public sealed class MantenimientoEquipoId : IntId { private MantenimientoEquipoId(int v) : base(v) { } public static MantenimientoEquipoId From(int v) => new(v); public static MantenimientoEquipoId New() => new(0); }
