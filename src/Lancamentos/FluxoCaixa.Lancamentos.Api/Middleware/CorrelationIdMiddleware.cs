namespace FluxoCaixa.Lancamentos.Api.Middleware;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            correlationId = Guid.NewGuid().ToString();

        context.Response.Headers.TryAdd(CorrelationIdHeader, correlationId.ToString());
        context.TraceIdentifier = correlationId.ToString();
        await _next(context);
    }
}
