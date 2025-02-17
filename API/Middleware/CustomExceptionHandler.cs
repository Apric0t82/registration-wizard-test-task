using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        int status = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        context.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = "An unhandled error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };

        await context.Response.WriteAsJsonAsync(problemDetails, token);

        return true;
    }
}