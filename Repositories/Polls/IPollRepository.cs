using Survey_Basket.Repositories;

namespace Survey_Basket.Repositories.Polls;

public interface IPollRepository : IGenericRepository<Poll>
{
    Task<bool> TogglePublishStatusAsync(Poll poll);
}
