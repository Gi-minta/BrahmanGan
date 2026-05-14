namespace BrahmanGan.Domain.Common;

// ============== FASE 5: PRODUCCIÓN DE LECHE ==============
public sealed class ParametroLactanciaId : IntId  { private ParametroLactanciaId(int v) : base(v) { } public static ParametroLactanciaId From(int v) => new(v); public static ParametroLactanciaId New() => new(0); }
