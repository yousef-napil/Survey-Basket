using Survey_Basket.Contracts.Polls;

namespace Survey_Basket.Services;

public interface IPollService
{
    Task<OneOf<IReadOnlyList<PollResponse>, Error>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<OneOf<PollResponse, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<OneOf<Poll, Error>> AddAsync(PollRequest entity, CancellationToken cancellationToken = default);

    Task<OneOf<bool, Error>> UpdateAsync(int id , PollRequest pollRequest, CancellationToken cancellationToken = default);

    Task<OneOf<bool, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<OneOf<bool, Error>> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);
}
