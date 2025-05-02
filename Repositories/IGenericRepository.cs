using Microsoft.EntityFrameworkCore.ChangeTracking;
using Survey_Basket.Specifications;

namespace Survey_Basket.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecifications<T> specifications , CancellationToken cancellationToken = default);
    Task<T> GetByIdAsyncWithSpec(ISpecifications<T> specifications, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
