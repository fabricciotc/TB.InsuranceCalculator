using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TB.Insurance.Domain.Common;

namespace TB.Insurance.Api.Middleware;

public sealed class DomainExceptionHandler(ILogger<DomainExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            DomainException => (StatusCodes.Status422UnprocessableEntity, "Business rule violation"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            _ => (0, string.Empty)
        };

        if (statusCode == 0)
            return false;

        logger.LogInformation(exception, "Request rejected with {StatusCode}", statusCode);

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            },
            cancellationToken);

        return true;
    }
}
