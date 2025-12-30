using ECommerce.Domain.ValueObjects;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order
{
    public string OrderNumber {get; set;} = string.Empty;
    public int UserId {get; set;}
    public User User {get; set;} = null!;
    public DateTime OrderDate {get; set;} = DateTime.UtcNow;
    public OrderStatus Status {get; set;} = OrderStatus.Pending;
    public Address ShippingAddress {get; set;} = null!;
    public Address BillingAddress {get; set;} = null!;

    // Navigation properties
    public ICollection<OrderItem> OrderItems {get; set;} = new List<OrderItem>();

    // Calculated properties
    public decimal Subtotal => OrderItems.Sum(x => x.TotalPrice);
    public decimal Tax => Subtotal * 0.18m; // %18 KDV (Burası özelleşecek.)
    public decimal Total => Subtotal + Tax;

    // Domain methods
    public void AddItem(Product product, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Cannot add items to a non-pending order");
        
        if(!product.IsInStock)
            throw new DomainException($"Product {product.Name} is out of stock");
        
        var existingItem = OrderItems.FirstOrDefault(x => x.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = quantity,
                UnitPrice = product.Price
            };
            OrderItems.Add(orderItem);
        }

        product.ReduceStock(quantity);
    }

    public void RemoveItem(int productId)
    {
        if(Status != OrderStatus.Pending)
            throw new DomainException("Cannot remove items from a non-pending order");
        
        var item = OrderItems.FirstOrDefault(x => x.ProductId == productId);
        if(item == null)
            throw new NotFoundException($"OrderItem with ProductId {productId} not found");
        OrderItems.Remove(item);
    }

    public void Confirm()
    {
        if(Status != OrderStatus.Pending)
            throw new DomainException("Only pending orders can be confirmed");
        
        if(!OrderItems.Any())
            throw new DomainException("Cannot confirm an order without items");

        Status = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if(Status != OrderStatus.Confirmed)
            throw new DomainException("Only confirmed orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if(Status != OrderStatus.Shipped)
            throw new DomainException("Only shipped orders can be delivered");
        Status = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainException("Cannot cancel a delivered order");
        if(Status == OrderStatus.Cancelled)
            throw new DomainException("Order is alreadt cancelled");

        Status = OrderStatus.Cancelled;
    }




}

