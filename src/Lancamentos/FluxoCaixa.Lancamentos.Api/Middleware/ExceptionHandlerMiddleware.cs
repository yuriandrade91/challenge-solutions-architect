using System.Text.Json;
using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Domain.Exceptions;

namespace FluxoCaixa.Lancamentos.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception: {Code} - {Message}", ex.Code, ex.Message);
            context.Response.StatusCode = ex.Code == "NOT_FOUND" ? 404 : 422;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Fail(ex.Code, ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Fail("INTERNAL_ERROR", "An internal error occurred.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
