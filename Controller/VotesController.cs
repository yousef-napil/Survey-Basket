using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Votes;
using Survey_Basket.Services;

namespace Survey_Basket.Controller;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class VotesController(IQuestionService questionService ,
                             IVoteService voteService) : ControllerBase
{
    private readonly IQuestionService questionService = questionService;
    private readonly IVoteService voteService = voteService;

    [HttpGet("")]
    public async Task<IActionResult> StartVote([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var result = await questionService.GetActiveAsync(pollId, userId! , cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }

    [HttpPost("")]
    public async Task<IActionResult> SaveVote([FromRoute] int pollId, [FromBody] VoteRequest voteRequest, CancellationToken cancellationToken)
    {
        var result = await voteService.SaveVote(pollId, User.GetUserId()!, voteRequest, cancellationToken);
        return result.Match<IActionResult>(
            success => Ok(),
            error => error.ToProblem(error.StatusCode));
    }

    [HttpGet("pollVotes")]
    public async Task<IActionResult> GetVotes([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await voteService.GetVotes(pollId, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }

    [HttpGet("pollVotesPerDay")]
    public async Task<IActionResult> GetVotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await voteService.GetVotesPerDayAsync(pollId, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }

    [HttpGet("pollVotesPerQuestion")]
    public async Task<IActionResult> GetVotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await voteService.GetVotesPerQuestionAsync(pollId, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }
}
