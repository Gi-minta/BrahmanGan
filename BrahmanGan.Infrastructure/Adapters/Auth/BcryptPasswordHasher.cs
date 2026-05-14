using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Infrastructure.Adapters.Auth;

/// <summary>Implementación de IPasswordHasher usando BCrypt.Net-Next.</summary>
public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hashear(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verificar(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}
