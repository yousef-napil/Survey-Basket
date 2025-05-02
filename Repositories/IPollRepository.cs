using Survey_Basket.Specifications.PollSpecifications;

namespace Survey_Basket.Repositories;

public interface IPollRepository : IGenericRepository<Poll>
{
    Task<bool> CheckPollTitleExists(PollTitleExistsSpec titleExists, CancellationToken cancellationToken = default);
}
