namespace BrahmanGan.Domain.Common;

/// <summary>
/// Value Object base para identidades tipadas.
/// Permite el valor por defecto (0 / Guid.Empty) para entidades aún no persistidas.
/// </summary>
public abstract class Identity<T> : ValueObject where T : notnull
{
    public T Value { get; }

    protected Identity(T value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool IsTransient() =>
        Value is null || Value.Equals(default(T));

    public override string ToString() => Value?.ToString() ?? string.Empty;

    public static implicit operator T(Identity<T> identity) => identity.Value;
}
