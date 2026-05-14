namespace BrahmanGan.Domain.Common;

// ============== SEGURIDAD (Auth, Roles, Permisos) ==============
public sealed class UsuarioId : IntId   { private UsuarioId(int v) : base(v) { } public static UsuarioId From(int v) => new(v); public static UsuarioId New() => new(0); }
