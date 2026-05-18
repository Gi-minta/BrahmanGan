// Common/UsuarioRolId.cs
namespace Common;

public sealed record UsuarioRolId(Guid Value)
{
    public static UsuarioRolId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
