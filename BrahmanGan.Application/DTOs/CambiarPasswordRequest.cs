namespace BrahmanGan.Application.DTOs;

/// <summary>Cambiar la contraseña del usuario autenticado.</summary>
public record CambiarPasswordRequest(
    string PasswordActual,
    string NuevoPassword,
    string ConfirmarNuevoPassword);
