using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Polls;
using Survey_Basket.Contracts.Question;
using Survey_Basket.Services;

namespace Survey_Basket.Controller;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService questionService = questionService;


    [HttpGet]
    public async Task<IActionResult> GetAllQuestions([FromRoute] int pollId , CancellationToken cancellationToken)
    {
        var result = await questionService.GetAllAsync(pollId, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }
    
    

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuestion([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await questionService.GetByIdAsync(pollId, id, cancellationToken);
        return result.Match<IActionResult>(
            Ok,
            error => error.ToProblem(error.StatusCode));
    }


    [HttpPost]
    public async Task<IActionResult> AddAsync([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await questionService.AddAsync(pollId, request, cancellationToken);
        return result.Match<IActionResult>(
            question => CreatedAtAction(nameof(GetQuestion), new { pollId , question.Id }, question.Adapt<QuestionResponse>()),
            error => error.ToProblem(error.StatusCode)
            );
    }

    [HttpPut("{questionId:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int pollId, [FromRoute] int questionId , [FromBody] QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await questionService.UpdateAsync(pollId, questionId , request , cancellationToken);
        return result.Match<IActionResult>(
                success => NoContent(),
                error => error.ToProblem(error.StatusCode)
        );
    }
}
