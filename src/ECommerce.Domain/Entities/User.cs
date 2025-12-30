using ECommerce.Domain.ValueObjects;
using ECommerce.Domain.Exceptions;
using System.Net.Sockets;

namespace ECommerce.Domain.Entities;

public class User : BaseEntity
{
    public string Email {get; set;} = string.Empty;
    public string PasswordHash {get; set;} = string.Empty;
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string? PhoneNumber {get; set;}
    public Address? ShippingAddress {get; set;}
    public Address? BillingAddress {get; set;}
    public bool IsActive {get; set;} = true;

    // Navigation properties
    public ICollection<Order> Orders {get; set;} = new List<Order>();

    public string FullName => $"{FirstName} {LastName}";
    
    public void DeActivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateShippingAddress(Address address)
    {
        ShippingAddress = address ?? throw new DomainException("Shipping address cannot be null.");
    }

    public void UpdateBillingAddress(Address address)
    {
        BillingAddress = address ?? throw new DomainException("Shipping address cannot be null.");
    }
}