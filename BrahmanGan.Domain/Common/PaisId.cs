namespace BrahmanGan.Domain.Common;

// ============== FASE 2: FINCA / GEOGRAFÍA ==============
public sealed class PaisId : IntId            { private PaisId(int v) : base(v) { } public static PaisId From(int v) => new(v); public static PaisId New() => new(0); }
