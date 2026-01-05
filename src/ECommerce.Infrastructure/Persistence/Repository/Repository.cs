


using ECommerce.Domain.Entities;
using ECommerce.Domain.Intrefaces;

namespace ECommerce.Infrastructure.Persistence.Repository;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
}