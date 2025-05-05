using Survey_Basket.Contracts.Question;

namespace Survey_Basket.Services;

public interface IQuestionService
{
    Task<OneOf<IReadOnlyList<QuestionResponse> , Error>> GetAllAsync(int pollId, CancellationToken cancellationToken = default);
    Task<OneOf<IReadOnlyList<QuestionResponse> , Error>> GetActiveAsync(int pollId, string userId , CancellationToken cancellationToken = default);

    Task<OneOf<QuestionResponse, Error>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken = default);

    Task<OneOf<Question, Error>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);

    Task<OneOf<bool, Error>> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellationToken = default);
}
