using Survey_Basket.Entities;
using Survey_Basket.Specifications;

namespace Survey_Basket.Repositories;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<bool> ContentExistsAsync(ISpecifications<Question> specifications, CancellationToken cancellationToken = default);
}
