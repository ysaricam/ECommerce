namespace ECommerce.Domain.Intrefaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products {get;}
    IUserRepository Users {get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}