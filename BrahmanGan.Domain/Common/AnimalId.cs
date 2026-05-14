namespace BrahmanGan.Domain.Common;

// ============== FASE 1: INVENTARIO ANIMAL ==============
public sealed class AnimalId : IntId          { private AnimalId(int v) : base(v) { } public static AnimalId From(int v) => new(v); public static AnimalId New() => new(0); }
