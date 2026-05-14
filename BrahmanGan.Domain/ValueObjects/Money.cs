using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.ValueObjects;

/// <summary>
/// Value Object para representar dinero con moneda
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0) 
            throw new DomainException("Amount cannot be negative");

        if (string.IsNullOrWhiteSpace(currency)) 
            throw new DomainException("Currency is required");

        return new Money(amount, currency.ToUpperInvariant());
    }

    public static Money Zero(string currency = "USD") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot add different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot subtract different currencies");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor) 
    {
        if (factor < 0) 
            throw new DomainException("Factor cannot be negative");

        return new Money(Amount * factor, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => string.Format("{0:N2} {1}", Amount, Currency);
}
