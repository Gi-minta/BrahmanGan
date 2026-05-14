using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace BrahmanGan.Domain.ValueObjects;

/// <summary>
/// Value Object para direcciones de correo electrónico.
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public string Address { get; }

    private Email(string address) { Address = address; }

    public static Email Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("El email no puede estar vacío");

        if (!EmailRegex.IsMatch(address))
            throw new DomainException($"Formato de email inválido: {address}");

        return new Email(address.ToLowerInvariant().Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Address;
    }

    public static implicit operator string(Email email) => email.Address;
    public override string ToString() => Address;
}
