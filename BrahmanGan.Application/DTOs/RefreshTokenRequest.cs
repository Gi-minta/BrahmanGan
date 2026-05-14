namespace BrahmanGan.Application.DTOs;

/// <summary>Renovar el access token usando el refresh token.</summary>
public record RefreshTokenRequest(string AccessToken, string RefreshToken);
