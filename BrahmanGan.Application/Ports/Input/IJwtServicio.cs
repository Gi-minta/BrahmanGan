using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

/// <summary>Contrato para generación y validación de JWT.</summary>
public interface IJwtServicio
{
    string GenerarAccessToken(UsuarioInfoResponse info);
    string GenerarRefreshToken();
    UsuarioInfoResponse? ValidarAccessToken(string token);
}
