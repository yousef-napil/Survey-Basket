using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Contracts.Polls;
using Survey_Basket.Repositories.Polls;

namespace Survey_Basket.Controller;
[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollRepository pollRepository;

    public PollsController(IPollRepository PollRepository)
    {
        pollRepository = PollRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPolls(CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetAllAsync(cancellationToken);
        return Ok(polls);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPoll([FromRoute] int id , CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll == null)
            return NotFound();
        return Ok(poll);
    }


    [HttpPost]
    public async Task<IActionResult> CreatePoll([FromBody] PollRequest poll, CancellationToken cancellationToken)
    {
        var mappedPoll = poll.Adapt<Poll>();
        var createdPoll = await pollRepository.AddAsync(mappedPoll, cancellationToken);
        if (createdPoll is null)
            return BadRequest("Poll creation failed.");
        return CreatedAtAction(nameof(GetPoll), new { id = createdPoll.Id }, createdPoll);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePoll([FromRoute] int id, [FromBody] PollRequest pollRequest, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll == null)
            return NotFound();
        var mappedPoll = pollRequest.Adapt(poll);
        var updated = await pollRepository.UpdateAsync(poll , cancellationToken);
        if (updated <= 0)
            return NotFound();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll == null)
            return NotFound();
        var deleted = await pollRepository.DeleteAsync(poll , cancellationToken);
        if (deleted <= 0)
            return BadRequest("Poll Deletion failed.");
        return NoContent();
    }

    [HttpPut("{id}/toggle-publish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(id, cancellationToken);
        if (poll == null)
            return NotFound();
        var result = await pollRepository.TogglePublishStatusAsync(poll);
        if (!result)
            return BadRequest("Poll publish status update failed.");
        return NoContent();
    }
}
