using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class OrderItem
{
    public int OrderId {get; set;}
    public Order Order {get; set;} = null!;
    public int ProductId { get; set; }
    public Product Product {get; set;} = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Calculated property
    public decimal TotalPrice => Quantity * UnitPrice;

    // Domain methods
    
    public void IncreaseQuantity(int amount)
    {
        if(amount <= 0)
            throw new DomainException("Amount must be greater than zero");
       
        Quantity += amount;
    }

    public void DecraseQuantity(int amount)
    {
        if (amount <= 0)
            throw new DomainException("Amount must be greater than zero");

        if(amount > Quantity)
            throw new DomainException("Cannot decrease quantity below zero");
        
        Quantity -= amount;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if(newQuantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        Quantity = newQuantity;
    }


}