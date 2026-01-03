using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount,string currency = "TRY")
    {
        if(amount < 0)
            throw new DomainException("Amount cannot be negative.");
        
        if(string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be null or empty.");
        
        Amount = amount;
        Currency = currency.ToUpper();
    }

    public Money Add(Money other)
    {
        if(Currency != other.Currency)
            throw new DomainException($"Cannot add different currencies: {Currency} and {other.Currency}.");
        
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if(Currency != other.Currency)
            throw new DomainException($"Cannot subtract different currencies: {Currency} and {other.Currency}.");
        
        var resultAmount = Amount - other.Amount;
        if(resultAmount < 0)
            throw new DomainException("Resulting amount cannot be negative.");
        
        return new Money(resultAmount, Currency);
    }

    public Money Multiply(decimal multipler)
    {
        return new Money (Amount * multipler, Currency);
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }

    public bool Equals(Money? other)
    {
        if(other is null)   return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public static bool operator ==(Money? left, Money? right)
    {
        if(left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }
}