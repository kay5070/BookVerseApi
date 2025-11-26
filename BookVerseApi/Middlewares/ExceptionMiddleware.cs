using System.Net;
using System.Text.Json;
using BookVerse.Infrastructure.Models;

namespace BookVerseApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // move to next middleware/controller
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new ErrorResponse
            {
                Status = 500,
                Message = _env.IsDevelopment() ? ex.Message : "An unexpected exception occurred.",
                StackTrace = _env.IsDevelopment() ? ex.StackTrace : null
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, options);

            await context.Response.WriteAsync(json);
        }
    }
}