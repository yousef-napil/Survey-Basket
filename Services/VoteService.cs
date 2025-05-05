using Survey_Basket.Contracts.Polls;
using Survey_Basket.Contracts.Results;
using Survey_Basket.Contracts.Votes;
using Survey_Basket.Repositories;

namespace Survey_Basket.Services;

public class VoteService(IPollRepository pollRepository ,
                         IVoteRepository voteRepository ,
                         IQuestionService questionService) : IVoteService
{
    private readonly IPollRepository pollRepository = pollRepository;
    private readonly IVoteRepository voteRepository = voteRepository;
    private readonly IQuestionService questionService = questionService;

    

    public async Task<OneOf<bool, Error>> SaveVote(int pollId , string userId , VoteRequest voteRequest, CancellationToken cancellationToken)
    {
        var availableQuestions = await questionService.GetActiveAsync(pollId, userId , cancellationToken);
        if (availableQuestions.IsT1)
            return availableQuestions.AsT1;

        var questionsIds = availableQuestions.AsT0.Select(x => x.Id).ToList();
        if (!voteRequest.VoteAnswers.Select(x => x.QuestionId).SequenceEqual(questionsIds))
            return VoteErrors.InvalidVote;

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = voteRequest.VoteAnswers.Adapt<ICollection<VoteAnswer>>()
        };

        var result = await voteRepository.AddAsync(vote, cancellationToken);
        if(vote is null)
            return VoteErrors.SaveFailed;

        return true;
    }

    public async Task<OneOf<VotesResponse, Error>> GetVotes(int pollId, CancellationToken cancellationToken)
    {
        var isPollExists = await pollRepository.GetByIdAsync(pollId, cancellationToken);
        if (isPollExists is null)
            return PollErrors.NotFound;
        var votes = await voteRepository.GetVotesAsync(pollId, cancellationToken);
        if (votes is null)
            return VoteErrors.NoVotes;
        return votes;

    }

    public async Task<OneOf<IEnumerable<VotesPerDay>, Error>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken)
    {
        var isPollExists = await pollRepository.GetByIdAsync(pollId, cancellationToken);
        if (isPollExists is null)
            return PollErrors.NotFound;
        var votes = await voteRepository.GetVotesPerDay(pollId, cancellationToken);
        if (votes is null)
            return VoteErrors.NoVotes;
        return OneOf<IEnumerable<VotesPerDay>, Error>.FromT0(votes!);
    }

    public async Task<OneOf<IEnumerable<VotesPerQuestionResponse>, Error>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken)
    {
        var votes = await voteRepository.GetVotesPerQuestionAsync(pollId, cancellationToken);
        if (votes is null)
            return VoteErrors.NoVotes;
        return OneOf<IEnumerable<VotesPerQuestionResponse>, Error>.FromT0(votes!);
    }
}
