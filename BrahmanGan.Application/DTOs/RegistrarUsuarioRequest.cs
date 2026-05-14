namespace BrahmanGan.Application.DTOs;

/// <summary>Registro de un nuevo usuario local.</summary>
public record RegistrarUsuarioRequest(
    string Email,
    string NombreCompleto,
    string Password,
    string ConfirmarPassword);
