namespace BrahmanGan.Application.DTOs;

// ══════════════════════════════════════════════════════════════
//  SEGURIDAD — Requests
// ══════════════════════════════════════════════════════════════

/// <summary>Login con email y contraseña.</summary>
public record LoginRequest(string Email, string Password);
