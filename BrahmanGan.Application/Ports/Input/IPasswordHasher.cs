namespace BrahmanGan.Application.Ports.Input;

/// <summary>Abstracción para hashing de contraseñas (BCrypt bajo el capó).</summary>
public interface IPasswordHasher
{
    string Hashear(string password);
    bool Verificar(string password, string hash);
}
