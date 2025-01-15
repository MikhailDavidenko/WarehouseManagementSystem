using Microsoft.AspNetCore.Mvc;

namespace WarehouseManagementSystem.Web.ManualClient;

public sealed class WebApiException: Exception
{
    public WebApiException(string message, ProblemDetails problemDetails)
        : base(message)
    {
        ProblemDetails = problemDetails;
    }

    public ProblemDetails ProblemDetails { get; }
}
