using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OneOf;
using Survey_Basket.Persistence;
using Survey_Basket.Specifications;

namespace Survey_Basket.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationContext context;

    public GenericRepository(ApplicationContext DbContext)
    {
        context = DbContext;
    }

    //public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    //=> await context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> specifications, CancellationToken cancellationToken = default)
    => await SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsNoTracking(), specifications).ToListAsync(cancellationToken);

    public async Task<T> GetByIdAsyncWithSpec(ISpecifications<T> specifications, CancellationToken cancellationToken = default)
    => await SpecificationEvaluator<T>.GetQuery(context.Set<T>(), specifications).FirstOrDefaultAsync(cancellationToken);

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    => await context.Set<T>().FindAsync(id);


    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var item = await context.Set<T>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return item.Entity;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Update(entity);
        var result = await context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
    public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Remove(entity);
        var result = await context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}
