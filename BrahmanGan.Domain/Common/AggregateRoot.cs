namespace BrahmanGan.Domain.Common;

/// <summary>
/// Raíz de agregado: punto de entrada para modificaciones.
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : class
{
    public int Version { get; protected set; }

    protected void IncrementVersion()
    {
        Version++;
        MarkAsModified();
    }
}
