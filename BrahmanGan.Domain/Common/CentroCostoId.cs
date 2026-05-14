namespace BrahmanGan.Domain.Common;

// ============== FASE 7: COSTOS / ALMACÉN / NÓMINA / SOSTENIBILIDAD / ICA / EQUIPOS ==============
public sealed class CentroCostoId : IntId       { private CentroCostoId(int v) : base(v) { } public static CentroCostoId From(int v) => new(v); public static CentroCostoId New() => new(0); }
