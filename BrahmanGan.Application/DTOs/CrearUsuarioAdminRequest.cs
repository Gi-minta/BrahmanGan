namespace BrahmanGan.Application.DTOs;

/// <summary>
/// Alta de usuario hecha por un administrador. La contraseña es temporal: el usuario
/// queda obligado a cambiarla la primera vez que entre.
/// </summary>
public record CrearUsuarioAdminRequest(
    string Email,
    string NombreCompleto,
    string PasswordTemporal,
    int RolId);

/// <summary>
/// Restablecimiento de contraseña hecho por un administrador, para cuando un usuario
/// pierde el acceso. La nueva contraseña también es temporal.
/// </summary>
public record RestablecerPasswordAdminRequest(string PasswordTemporal);
