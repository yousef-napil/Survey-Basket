using Survey_Basket.Contracts.Polls;
using Survey_Basket.Contracts.Results;
using Survey_Basket.Contracts.Votes;

namespace Survey_Basket.Services;

public interface IVoteService
{
    Task<OneOf<bool , Error>> SaveVote (int pollId , string userId , VoteRequest voteRequest, CancellationToken cancellationToken);

    Task<OneOf<VotesResponse , Error>> GetVotes(int pollId, CancellationToken cancellationToken);

    Task<OneOf<IEnumerable<VotesPerDay> , Error>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken);
    Task<OneOf<IEnumerable<VotesPerQuestionResponse> , Error>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken);
}
