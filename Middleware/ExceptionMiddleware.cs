using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models.Exceptions;

namespace Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        //todo logger
        ExceptionDetails exceptionDetails = new();
        int statusCode = 404;
        if (ex is CustomException customEx)
        {
            statusCode = (int)customEx.StatusCode;
            exceptionDetails.title = customEx.GetType().ShortDisplayName().Split("<")[0]; // ShortDisplayName = 'GenericException<typeof>'
            exceptionDetails.messege = customEx.Message;
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            exceptionDetails.title = "Internal Server Error";
            exceptionDetails.messege = "Internal server error occurred!";
        }

        exceptionDetails.status = statusCode;
        exceptionDetails.traceId = context.Request.Path;

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(exceptionDetails);
        await context.Response.WriteAsync(result);
    }
}

internal record ExceptionDetails()
{
    public int status { get; set; }
    public string title { get; set; }
    public string messege { get; set; }
    public string traceId { get; set; }
};