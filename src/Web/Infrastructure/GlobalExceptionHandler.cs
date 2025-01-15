using WarehouseManagementSystem.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WarehouseManagementSystem.Web.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ProblemDetailsFactory problemDetailsFactory;

    public GlobalExceptionHandler(ProblemDetailsFactory problemDetailsFactory)
    {
        this.problemDetailsFactory = problemDetailsFactory;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            title: exception.GetType().Name,
            detail: exception.Message);

        problemDetails.Status = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            BusinessLogicException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
