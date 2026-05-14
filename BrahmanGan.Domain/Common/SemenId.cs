namespace BrahmanGan.Domain.Common;

// ============== FASE 3: REPRODUCCIÓN ==============
public sealed class SemenId : IntId           { private SemenId(int v) : base(v) { } public static SemenId From(int v) => new(v); public static SemenId New() => new(0); }
