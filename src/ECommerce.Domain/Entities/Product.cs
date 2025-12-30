using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name {get; set;} = string.Empty;
    public string? Description {get; set;}
    public string Sku {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public int StockQuantity {get; set;}
    public ProductStatus Status {get; set;} = ProductStatus.Draft;
    public int CategoryId {get; set;}

    // Domain methods
    public void Activate()
    {
        if(Status == ProductStatus.Draft)
            Status = ProductStatus.Active;
    }

    public void DeActivate()
    {
        Status = ProductStatus.InActive;
    }

    public bool IsInStock => StockQuantity > 0;

    public void ReduceStock(int quantity)
    {
        if(quantity > StockQuantity)
            throw new DomainException($"Insufficient stock. Available: {StockQuantity}, Requested: {quantity}");
        
        StockQuantity -= quantity;
    }
}