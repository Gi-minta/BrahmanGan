using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Exceptions;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace BrahmanGan.Infrastructure.Adapters.Auth;

/// <summary>
/// Valida ID tokens de Google con <c>Google.Apis.Auth</c>, que comprueba la firma contra las
/// claves públicas de Google, el emisor y la caducidad. Aquí se añade la comprobación de
/// audiencia frente al ClientId propio y la de correo verificado.
/// </summary>
public sealed class GoogleTokenValidator : IGoogleTokenValidator
{
    private readonly string? _clientId;

    public GoogleTokenValidator(IConfiguration configuration)
    {
        _clientId = configuration["OAuth:Google:ClientId"]?.Trim();
    }

    public bool EstaConfigurado => !string.IsNullOrWhiteSpace(_clientId);

    public async Task<GoogleIdentidad> ValidarAsync(string idToken, CancellationToken ct = default)
    {
        if (!EstaConfigurado)
            throw new BusinessRuleException(
                "El inicio de sesión con Google no está disponible: falta configurar " +
                "OAuth:Google:ClientId.");

        if (string.IsNullOrWhiteSpace(idToken))
            throw new BusinessRuleException("Falta el ID token de Google.");

        GoogleJsonWebSignature.Payload payload;
        try
        {
            // Audience acotada a nuestro ClientId: sin esto se aceptaría un token emitido
            // para cualquier otra aplicación de Google, y quien lo presentara entraría aquí.
            payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _clientId! }
                });
        }
        catch (InvalidJwtException ex)
        {
            throw new BusinessRuleException($"El ID token de Google no es válido: {ex.Message}");
        }

        // Google puede devolver una cuenta cuyo correo no ha sido verificado; darla por buena
        // permitiría reclamar el correo de otra persona.
        if (!payload.EmailVerified)
            throw new BusinessRuleException("La cuenta de Google no tiene el correo verificado.");

        if (string.IsNullOrWhiteSpace(payload.Email))
            throw new BusinessRuleException("El ID token de Google no incluye un correo.");

        if (string.IsNullOrWhiteSpace(payload.Subject))
            throw new BusinessRuleException("El ID token de Google no incluye el identificador de cuenta.");

        return new GoogleIdentidad(
            payload.Email,
            string.IsNullOrWhiteSpace(payload.Name) ? payload.Email : payload.Name,
            payload.Subject);
    }
}
