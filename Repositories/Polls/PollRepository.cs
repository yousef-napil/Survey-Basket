using Survey_Basket.Persistence;

namespace Survey_Basket.Repositories.Polls;

public class PollRepository : GenericRepository<Poll>, IPollRepository
{
    private readonly ApplicationContext context;

    public PollRepository(ApplicationContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<bool> TogglePublishStatusAsync(Poll poll)
    {
        poll.IsPublished = !poll.IsPublished;
        context.Polls.Update(poll);
        var result = await context.SaveChangesAsync();
        if (result > 0)
            return true;
        return false;
    }
}
