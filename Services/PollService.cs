using Survey_Basket.Contracts.Authentication;
using Survey_Basket.Contracts.Polls;
using Survey_Basket.Entities;
using Survey_Basket.Repositories;
using Survey_Basket.Specifications;
using Survey_Basket.Specifications.PollSpecifications;

namespace Survey_Basket.Services;

public class PollService : IPollService
{
    private readonly IPollRepository pollRepository;

    public PollService(IPollRepository pollRepository)
    {
        this.pollRepository = pollRepository;
    }


    public async Task<OneOf<IReadOnlyList<PollResponse>, Error>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var spec = new PollSpec();
        var polls = await pollRepository.GetAllAsyncWithSpec(spec , cancellationToken);
        if (polls.Count == 0)
            return PollErrors.NotFound;
        var result = polls.Adapt<IReadOnlyList<PollResponse>>();
        return OneOf<IReadOnlyList<PollResponse>, Error>.FromT0(result);
    }

    public async Task<OneOf<IReadOnlyList<PollResponse>, Error>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        
        var spec = new CurrentPollSpec();
        var polls = await pollRepository.GetAllAsyncWithSpec(spec, cancellationToken);
        if (polls.Count == 0)
            return PollErrors.NotFound;
        var result = polls.Where(x => x.IsPublished).Adapt<IReadOnlyList<PollResponse>>();
        return OneOf<IReadOnlyList<PollResponse>, Error>.FromT0(result);
    }


    public async Task<OneOf<PollResponse, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var spec = new PollSpec(id);
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll == null)
            return PollErrors.NotFound;
        var result = poll.Adapt<PollResponse>();
        return result;
    }
    public async Task<OneOf<Poll, Error>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var spec = new PollTitleExistsSpec(pollRequest.Title);
        var pollExists = await pollRepository.CheckPollTitleExists(spec, cancellationToken);
        if (pollExists)
            return PollErrors.AlreadyExists;
        var poll = pollRequest.Adapt<Poll>();
        var result = await pollRepository.AddAsync(poll, cancellationToken);
        if (result is null)
            return PollErrors.CreationFailed;
        return result;
    }

    public async Task<OneOf<bool, Error>> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var spec = new PollTitleExistsSpec(pollRequest.Title , id);
        var pollExists = await pollRepository.CheckPollTitleExists(spec, cancellationToken);
        if (pollExists)
            return PollErrors.AlreadyExists;
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll is null) 
            return PollErrors.NotFound;
        var mappedPoll = pollRequest.Adapt(poll);
        var isSuccess = await pollRepository.UpdateAsync(mappedPoll, cancellationToken);
        return isSuccess ? true : PollErrors.UpdateFailed;
    }

    public async Task<OneOf<bool, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll is null)
            return PollErrors.NotFound;
        var isSuccess = await pollRepository.DeleteAsync(poll, cancellationToken);
        return isSuccess ? true : PollErrors.DeletionFailed;
    }

    public async Task<OneOf<bool, Error>> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll is null)
            return PollErrors.NotFound;
        poll.IsPublished = !poll.IsPublished;
        var isSuccess = await pollRepository.UpdateAsync(poll, cancellationToken);
        return isSuccess ? true : PollErrors.DeletionFailed;
    }
}
