using Survey_Basket.Contracts.Question;
using Survey_Basket.Persistence;
using Survey_Basket.Specifications;

namespace Survey_Basket.Repositories;

public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
{
    private readonly ApplicationContext context;

    public QuestionRepository(ApplicationContext context) : base(context)
    {
        this.context = context;
    }

    public Task<bool> ContentExistsAsync(ISpecifications<Question> specifications, CancellationToken cancellationToken = default)
    =>context.Questions.AnyAsync(specifications.Any, cancellationToken);

    //public Task<bool> ContentExistsAsync(int pollId, CancellationToken cancellationToken = default)
    //{
    //    Type questionRespones = typeof(QuestionResponse);
    //    var questions = context.Questions.AsNoTracking()
    //                                     .Where(q => q.PollId == pollId)
    //                                     .Include(q => q.Answers)
    //                                     .ProjectToType(questionRespones); 
    //}
}
