namespace BrahmanGan.Domain.Common;

// ============== FASE 4: SANIDAD ==============
public sealed class MedicamentoId : IntId               { private MedicamentoId(int v) : base(v) { } public static MedicamentoId From(int v) => new(v); public static MedicamentoId New() => new(0); }
