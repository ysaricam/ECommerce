namespace ECommerce.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;  // Soft delete
}

public abstract class BaseEntity<TId> : BaseEntity
{
    public new TId Id { get; set; } = default!;
}