namespace BrahmanGan.Application.DTOs;

// ══════════════════════════════════════════════════════════════
//  SEGURIDAD — Responses
// ══════════════════════════════════════════════════════════════

/// <summary>Resultado del login / refresh — contiene los tokens.</summary>
public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime Expira,
    UsuarioInfoResponse Usuario);
