
namespace Survey_Basket.Specifications.PollSpecifications;

public class CurrentPollSpec : BaseSpecification<Poll>
{
    public CurrentPollSpec()
    {
        AddCriteria( x => x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) );
    }
}
