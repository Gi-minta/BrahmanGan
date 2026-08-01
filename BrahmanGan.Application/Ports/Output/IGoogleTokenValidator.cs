namespace BrahmanGan.Application.Ports.Output;

/// <summary>Identidad extraída de un ID token de Google ya validado.</summary>
/// <param name="Email">Correo verificado por Google.</param>
/// <param name="NombreCompleto">Nombre del perfil, o el correo si Google no lo envía.</param>
/// <param name="IdExterno">El <c>sub</c> de Google: identificador estable de la cuenta.</param>
public record GoogleIdentidad(string Email, string NombreCompleto, string IdExterno);

/// <summary>
/// Puerto de salida para validar un ID token de Google.
/// </summary>
/// <remarks>
/// La identidad debe salir <b>siempre</b> del token firmado por Google, nunca del cuerpo de
/// la petición: el correo y el identificador que envía un cliente son texto sin verificar y
/// aceptarlos permitiría emitir tokens propios a cualquiera.
/// </remarks>
public interface IGoogleTokenValidator
{
    /// <summary>Indica si hay un ClientId configurado. Sin él no se puede validar nada.</summary>
    bool EstaConfigurado { get; }

    /// <summary>
    /// Valida la firma, el emisor, la caducidad y la audiencia del token.
    /// </summary>
    /// <returns>La identidad verificada.</returns>
    /// <exception cref="Domain.Exceptions.BusinessRuleException">
    /// Si el token no es válido, la audiencia no corresponde al ClientId configurado, el
    /// correo no está verificado en Google, o no hay ClientId configurado.
    /// </exception>
    Task<GoogleIdentidad> ValidarAsync(string idToken, CancellationToken ct = default);
}
