using System.Security.Cryptography;

namespace BrahmanGan.API.Extensions;

/// <summary>
/// Garantiza que exista una clave de firma JWT (<c>Jwt:SecretKey</c>) sin credenciales
/// hardcodeadas en el repositorio.
///
/// - En <b>producción</b> la clave es obligatoria: si falta, la aplicación falla al arrancar.
/// - En <b>Development</b>, si no se configura, se genera una clave EFÍMERA (compartida por la
///   firma y la validación) para no bloquear el desarrollo/pruebas. Los tokens se invalidan al
///   reiniciar; define <c>Jwt__SecretKey</c> para fijarla.
///
/// Configúrala de forma segura: variable de entorno <c>Jwt__SecretKey</c>, user-secrets
/// (<c>dotnet user-secrets set "Jwt:SecretKey" ...</c>) o un appsettings NO versionado.
/// </summary>
public static class JwtKeyBootstrap
{
    /// <summary>HMAC-SHA256 requiere una clave de al menos 256 bits (32 caracteres).</summary>
    private const int MinKeyLength = 32;

    public static void EnsureJwtSecretKey(IConfiguration config, IHostEnvironment env)
    {
        var key = config["Jwt:SecretKey"];

        if (!string.IsNullOrWhiteSpace(key))
        {
            if (key.Length < MinKeyLength)
                throw new InvalidOperationException(
                    $"Jwt:SecretKey es demasiado corta ({key.Length} caracteres); usa al menos {MinKeyLength}.");
            return;
        }

        if (!env.IsDevelopment())
            throw new InvalidOperationException(
                "Jwt:SecretKey no está configurada. Defínela de forma segura (variable de entorno " +
                "Jwt__SecretKey, user-secrets o un appsettings no versionado). " +
                $"Debe tener al menos {MinKeyLength} caracteres.");

        // Development: clave efímera compartida por firma y validación (se mantiene en
        // la configuración en memoria para que ambos la lean igual durante esta ejecución).
        var generada = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        config["Jwt:SecretKey"] = generada;

        Console.WriteLine(
            "[JwtKeyBootstrap] ADVERTENCIA: Jwt:SecretKey no configurada; se generó una clave " +
            "EFÍMERA para Development (los tokens se invalidan al reiniciar). " +
            "Define Jwt__SecretKey para fijarla.");
    }
}
