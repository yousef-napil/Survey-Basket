using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Polls;
using Survey_Basket.Repositories;
using Survey_Basket.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Survey_Basket.Controller;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController : ControllerBase
{
    private readonly IPollRepository pollRepository;
    private readonly IPollService pollService;

    public PollsController(IPollRepository PollRepository , IPollService pollService)
    {
        pollRepository = PollRepository;
        this.pollService = pollService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPolls(CancellationToken cancellationToken)
    {
        var pollResults = await pollService.GetAllAsync(cancellationToken);
        return pollResults.Match(
            Ok,
            error => error.ToProblem(StatusCodes.Status404NotFound)
        );
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPoll([FromRoute] int id , CancellationToken cancellationToken)
    {
        var poll = await pollService.GetByIdAsync(id, cancellationToken);
        return poll.Match(
            poll => Ok(poll.Adapt<PollResponse>()),
            error => error.ToProblem(error.StatusCode)
        );
    }


    [HttpPost]
    public async Task<IActionResult> CreatePoll([FromBody] PollRequest poll, CancellationToken cancellationToken)
    {
        var createdPoll = await pollService.AddAsync(poll, cancellationToken);
        return createdPoll.Match(
            poll => CreatedAtAction(nameof(GetPoll), new { id = poll.Id }, poll.Adapt<PollResponse>()),
            error => error.ToProblem(error.StatusCode)
        );
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest pollRequest, CancellationToken cancellationToken)
    {
        var result = await pollService.UpdateAsync(id , pollRequest , cancellationToken);
        return result.Match<IActionResult>(
                success => NoContent(),
                error => error.ToProblem(error.StatusCode)
        );        
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await pollService.DeleteAsync(id, cancellationToken);
        return result.Match<IActionResult>(
                success => NoContent(),
                error => error.ToProblem(error.StatusCode)
        );
    }

    [HttpPut("{id}/toggle-publish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await pollService.TogglePublishStatusAsync(id , cancellationToken);
        return result.Match<IActionResult>(
                success => NoContent(),
                error => error.ToProblem(error.StatusCode)
        );
    }
}
