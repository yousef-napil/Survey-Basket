using Microsoft.AspNetCore.Mvc;

namespace Survey_Basket.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem (this Error error , int statusCode)
    {
        var problem = Results.Problem(statusCode : statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors" , new [] { error }
            }
        };

        return new ObjectResult(problemDetails);
    }
}
