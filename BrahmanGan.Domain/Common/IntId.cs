namespace BrahmanGan.Domain.Common;

/// <summary>
/// Identidades tipadas de las entidades del dominio.
/// Convención: cada entidad tiene su propio Id tipado para evitar mezclar enteros.
/// </summary>
public abstract class IntId : Identity<int>
{
    protected IntId(int value) : base(value) { }
}
