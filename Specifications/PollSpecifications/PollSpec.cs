
namespace Survey_Basket.Specifications.PollSpecifications;

public class PollSpec : BaseSpecification<Poll>
{
    public PollSpec()
    {
        
    }

    public PollSpec(int id)
    {
        AddCriteria(x => x.Id == id);
    }

    
}
