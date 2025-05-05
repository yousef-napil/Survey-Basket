using Survey_Basket.Contracts.Results;

namespace Survey_Basket.Repositories;

public interface IVoteRepository : IGenericRepository<Vote>
{
    Task<bool> HasVoted(int pollId, string userId, CancellationToken cancellationToken = default);
    Task<VotesResponse?> GetVotesAsync(int pollId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VotesPerDay?>> GetVotesPerDay(int pollId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VotesPerQuestionResponse>?> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
}
