using Microsoft.AspNetCore.Mvc;
using Pho.Core.Exceptions;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Pho.Web.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ThirdPartyServiceException ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;

            var response = new ProblemDetails { Status = context.Response.StatusCode, Detail = ex.Message };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
