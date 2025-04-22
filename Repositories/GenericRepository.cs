using Survey_Basket.Persistence;

namespace Survey_Basket.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationContext context;

    public GenericRepository(ApplicationContext DbContext)
    {
        context = DbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    => await context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id , CancellationToken cancellationToken = default)
    => await context.Set<T>().FindAsync(id, cancellationToken);

    public async Task<T?> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var item = await context.Set<T>().AddAsync(entity , cancellationToken);
        var result = await context.SaveChangesAsync(cancellationToken);
        if (result > 0)
            return await GetByIdAsync(item.Entity.Id);
        return null;
    }

    public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Update(entity);
        return await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Remove(entity);
        return await context.SaveChangesAsync(cancellationToken);
    }
}
