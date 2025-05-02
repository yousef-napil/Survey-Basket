namespace Survey_Basket.Specifications.PollSpecifications;

public class PollTitleExistsSpec : BaseSpecification<Poll>
{
    public PollTitleExistsSpec(string title)
    {
        AddAny(p => p.Title == title);
    }

    public PollTitleExistsSpec(string title, int id)
    {
        AddAny(p => p.Title == title && p.Id != id);
    }
}
