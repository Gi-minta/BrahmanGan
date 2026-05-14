namespace BrahmanGan.Domain.Common;

// ============== FASE 6: COMERCIAL ==============
public sealed class ClienteId : IntId           { private ClienteId(int v) : base(v) { } public static ClienteId From(int v) => new(v); public static ClienteId New() => new(0); }
