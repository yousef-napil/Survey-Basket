using Survey_Basket.Contracts.Results;
using Survey_Basket.Persistence;

namespace Survey_Basket.Repositories;

public class VoteRepository(ApplicationContext context) : GenericRepository<Vote>(context), IVoteRepository
{
    private readonly ApplicationContext context = context;

    public async Task<bool> HasVoted(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        return await context.Votes
            .AnyAsync(v => v.PollId == pollId && v.UserId == userId);
    }

    public async Task<VotesResponse?> GetVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await context.Polls
            .Where(p => p.Id == pollId)
            .Select(p => new VotesResponse(
                    p.Title,
                    p.Votes.Select(v => new UserVotes(
                            $"{v.User.FirstName} {v.User.LastName}",
                            v.VoteAnswers.Select(qa => new QuestionAnswers(
                                    qa.Question.Content,
                                    qa.Answer.Content
                                ))
                    ))
            ))
            .FirstOrDefaultAsync(cancellationToken);


    }

    public async Task<IEnumerable<VotesPerDay?>> GetVotesPerDay(int pollId, CancellationToken cancellationToken = default)
    {
        return await context.Votes
            .Where(v => v.PollId == pollId)
            .GroupBy(v => v.SubmittedOn)
            .Select(g => new VotesPerDay(
                    DateOnly.FromDateTime(g.Key),
                    g.Count()
            ))
            .ToListAsync();          
    }

    public async Task<IEnumerable<VotesPerQuestionResponse>?> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {
        return await context.VoteAnswers
            .Where(v => v.Vote.PollId == pollId)
            .Select(x => new VotesPerQuestionResponse (
                    x.Question.Content,
                    x.Question.Votes.GroupBy(v => new { AnswerId = v.AnswerId , AnswerContent = v.Answer.Content })
                                    .Select(a => new SelectedAnswers (
                                        a.Key.AnswerContent,
                                        a.Count()
                                    ))
                ))
            .ToListAsync(cancellationToken);

    }
}
