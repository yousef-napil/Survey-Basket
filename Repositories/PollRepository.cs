using Survey_Basket.Persistence;
using Survey_Basket.Specifications.PollSpecifications;

namespace Survey_Basket.Repositories;

public class PollRepository : GenericRepository<Poll>, IPollRepository
{
    private readonly ApplicationContext context;

    public PollRepository(ApplicationContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<bool> CheckPollTitleExists(PollTitleExistsSpec titleExists, CancellationToken cancellationToken = default)
    => await context.Polls.AnyAsync(titleExists.Any, cancellationToken);

    public async Task<bool> IsPollActive(int pollId, CancellationToken cancellationToken = default)
    =>await context.Polls.AnyAsync(x => x.Id == pollId && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow));
}
