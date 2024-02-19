using Microsoft.AspNetCore.Mvc;

namespace OldWay.GlobalExceptionHandlingMiddleware;

public class GlobalExceptionHandler(
    RequestDelegate next,
    ILogger<GlobalExceptionHandler> logger)
{
    /// <summary>
    /// a <see cref="RequestDelegate"/> that is used to invoke the next middleware in the pipeline
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error"
            };

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
