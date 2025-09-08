public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestLoggingMiddleware> logger;

    public RequestLoggingMiddleware(RequestDelegate _next, ILogger<RequestLoggingMiddleware> _logger)
    {
        next = _next;
        logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

        await next(context);

        logger.LogInformation($"Response: {context.Response.StatusCode} processed");
    }

}