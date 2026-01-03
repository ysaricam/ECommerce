using System.Reflection;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name {get; private set;} = string.Empty;
    public string? Description {get; private set;}
    public string Sku {get; private set;} = string.Empty;
    public decimal Price {get; private set;}
    public int StockQuantity {get; private set;}
    public ProductStatus Status {get; private set;} = ProductStatus.Draft;
    public int CategoryId {get; private set;}

    // Navigation properties
    public ICollection<OrderItem> OrderItems {get; private set; } = new List<OrderItem>();

    // Calculated properties
    public bool IsInStock => StockQuantity > 0 && Status == ProductStatus.Active;
    public bool IsActive => Status == ProductStatus.Active;

    // Paramaterless constructor for EF Core
    private Product() {}

    // Factory method for creating new products
    public static Product Create(string name, string sku, decimal price, int categoryId, string? description = null)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty");
        if(string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU cannot be empty");
        if(price < 0)
            throw new DomainException("Product price cannot be negative");
        if(categoryId <= 0)
            throw new DomainException("Invalid category ID");

        return new Product
        {
          Name = name,
          Sku = sku,
          Price = price,
          CategoryId = categoryId,
          Description = description,
          StockQuantity = 0,
          Status = ProductStatus.Draft  
        };
    }

    // Domain methods for state changes
    public void UpdateDetails(string name, string? description)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty");
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if(newPrice < 0)
            throw new DomainException("Product price cannot be negative");
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSku(string newSku)
    {
        if(string.IsNullOrWhiteSpace(newSku))
            throw new DomainException("Product SKU cannot be empty");
        Sku = newSku;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeCategory(int newCategoryId)
    {
        if(newCategoryId <= 0)
            throw new DomainException("Invalid category ID");
        
        CategoryId = newCategoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if(Status == ProductStatus.Discontinued)
            throw new DomainException("Cannot activate a discontiuned prodyct");
        if(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Sku))
            throw new DomainException("Cannot activate prodyct without name and SKU");
        if(Price <= 0)
            throw new DomainException("Cannot activate product with invalid price");

        Status = ProductStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if(Status == ProductStatus.Discontinued)
            throw new DomainException("Product is already discontiuned");

        Status = ProductStatus.InActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Discontinue()
    {

        Status = ProductStatus.Discontinued;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if(quantity <= 0)
            throw new DomainException("Quantity to add must be greater than zero");
        
        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReduceStock(int quantity)
    {
        if(quantity <= 0)
            throw new DomainException("Quantity to reduce must be greater than zero");
        
        if(quantity > StockQuantity)
            throw new DomainException($"Insufficient stock. Available : {StockQuantity}, Requested : {quantity}");
        
        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetStock(int quantity)
    {
        if(quantity < 0)
            throw new DomainException("Stock quantity cannot be negative");
        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanBePurchased(int requestedQuantity)
    {
        return IsInStock &&
            StockQuantity >= requestedQuantity &&
            requestedQuantity > 0;
    }

    public Money GetPriceAsMoney(string currency = "TRY")
    {
        return new Money(Price, currency);
    }
}