using Mapster;
using Survey_Basket.Contracts.Question;
using Survey_Basket.Repositories;
using Survey_Basket.Specifications.PollSpecifications;
using Survey_Basket.Specifications.QuestionSpecifications;

namespace Survey_Basket.Services;

public class QuestionService(IPollRepository pollRepository, 
                             IQuestionRepository questionRepository ,
                             IVoteRepository voteRepository
                             ) : IQuestionService
{
    private readonly IPollRepository pollRepository = pollRepository;
    private readonly IQuestionRepository questionRepository = questionRepository;
    private readonly IVoteRepository voteRepository = voteRepository;

    public async Task<OneOf<IReadOnlyList<QuestionResponse>, Error>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await pollRepository.GetByIdAsync(pollId, cancellationToken);
        if (pollIsExists is null)
            return PollErrors.NotFound;
        var questions = await questionRepository.GetAllAsyncWithSpec(new QuestionSpec(pollId), cancellationToken);
        if (questions is null)
            return QuestionErrors.NotFound;
        return OneOf<IReadOnlyList<QuestionResponse>, Error>.FromT0(questions.Adapt<IReadOnlyList<QuestionResponse>>());
    }

    public async Task<OneOf<IReadOnlyList<QuestionResponse>, Error>> GetActiveAsync(int pollId, string userId , CancellationToken cancellationToken = default)
    {
        var isPollActive = await pollRepository.IsPollActive(pollId, cancellationToken);
        if (!isPollActive)
            return PollErrors.NotFound;
        var hasVoted = await voteRepository.HasVoted(pollId, userId, cancellationToken);
        if (hasVoted)
            return VoteErrors.AlreadyVoted;
        var questions = await questionRepository.GetAllAsyncWithSpec(new ActiveQuestionSpec(pollId), cancellationToken);
        
        if (questions is null)
            return QuestionErrors.NotFound;
        
        return OneOf<IReadOnlyList<QuestionResponse>, Error>.FromT0(questions.Adapt<IReadOnlyList<QuestionResponse>>());
    }

    public async Task<OneOf<QuestionResponse, Error>> GetByIdAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await pollRepository.GetByIdAsync(pollId, cancellationToken);
        if (pollIsExists is null)
            return PollErrors.NotFound;
        var question = await questionRepository.GetByIdAsyncWithSpec(new QuestionSpec(pollId, questionId), cancellationToken);
        if (question is null)
            return QuestionErrors.NotFound;
        return question.Adapt<QuestionResponse>();
    }

    public async Task<OneOf<Question, Error>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await pollRepository.GetByIdAsync(pollId, cancellationToken);
        if (pollIsExists is null)
            return PollErrors.NotFound;
        var questionExists = await questionRepository.ContentExistsAsync(new QuestionContentExistsSpec(pollId, request.Content), cancellationToken);
        if (questionExists)
            return QuestionErrors.AlreadyExists;
        var mappedQuestion = request.Adapt<Question>();
        mappedQuestion.PollId = pollId;
        var question = await questionRepository.AddAsync(mappedQuestion, cancellationToken);
        if (question is null)
            return QuestionErrors.CreationFailed;
        return question;
    }

    public async Task<OneOf<bool, Error>> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var questionExists = await questionRepository.ContentExistsAsync(new QuestionContentExistsSpec(pollId, request.Content , questionId), cancellationToken);
        if (questionExists)
            return QuestionErrors.AlreadyExists;
        var question = await questionRepository.GetByIdAsyncWithSpec(new QuestionSpec(pollId, questionId), cancellationToken);
        if (question is null)
            return QuestionErrors.NotFound;
        //question.Content = request.Content;
        /*
         *  A1 A2 => currentAnswers
         *  A1 A3 => Request
         *  A3 => newAnswers
         */
        var currentAnswers = question.Answers.Select(q => q.Content).ToList();
        var newAnswers = request.Answers.Except(currentAnswers).ToList();
        newAnswers.ForEach(a =>
        {
            question.Answers.Add(new Answer { Content = a });
        });
        question.Answers.ForEach(a =>
        {
            a.IsActive = request.Answers.Contains(a.Content);
        });
        var isSuccess = await questionRepository.UpdateAsync(question, cancellationToken);
        if (!isSuccess)
            return QuestionErrors.UpdateFailed;
        return isSuccess;
    }
}
