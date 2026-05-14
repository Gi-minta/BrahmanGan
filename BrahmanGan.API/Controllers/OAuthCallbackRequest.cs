namespace BrahmanGan.API.Controllers;

/// <summary>Payload que envía el frontend tras el flujo OAuth2.</summary>
public record OAuthCallbackRequest(
    string Email,
    string NombreCompleto,
    string IdExterno,
    string? IdToken = null);
