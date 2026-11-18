using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SmartEMR.ApiPractice.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not OperationCanceledException || httpContext.Response.HasStarted)
        {
            return false;
        }

        httpContext.Response.StatusCode = 499;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = 499,
            Title = "요청이 취소되었습니다.",
            Detail = "클라이언트 요청이 취소되어 작업을 중단했습니다.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        Debug.WriteLine("요청 취소 발생");

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken: CancellationToken.None);

        return true;
    }
}
