namespace ECommerce.Domain.ValueObjects;

public class Address
{
    public string Street {get; private set;}
    public string City {get; private set;}
    public string State {get; private set;}
    public string ZipCode {get; private set;}
    public string Country {get; private set;}

    public Address(string street, string city, string state, string zipCode, string country)
    {
        if(string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be null or empty.");
        
        if(string.IsNullOrWhiteSpace(city))
            throw new DomainException("City cannot be null or empty.");
        
        if(string.IsNullOrWhiteSpace(state))
            throw new DomainException("State cannot be null or empty.");
        
        if(string.IsNullOrWhiteSpace(zipCode))
            throw new DomainException("ZipCode cannot be null or empty.");
        
        if(string.IsNullOrWhiteSpace(country))
            throw new DomainException("Country cannot be null or empty.");
        
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    public string FullAddress => $"{Street}, {City}, {State}, {ZipCode}, {Country}";

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street && City == other.City &&
               State == other.State && ZipCode == other.ZipCode &&
               Country == other.Country;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode, Country);
    }

    public static bool operator ==(Address? left, Address? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }
    
    public static bool operator !=(Address? left, Address? right)
    {
        return !(left == right);
    }
}