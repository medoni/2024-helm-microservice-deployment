namespace PizzaService.Aws.Services.AspNet;

internal class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger
)
{

    public async Task InvokeAsync(HttpContext context)
    {
        var logScope = logger.BeginScope("Processing request ...");
        try
        {
            logger.LogInformation("Processing request {method}, '{path}' ...", context.Request.Method, context.Request.Path.Value);
            await next(context);
            logger.LogInformation("Successfully processed request {method}, '{path}' ...", context.Request.Method, context.Request.Path.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing request {method}, '{path}' ...", context.Request.Method, context.Request.Path.Value);
            throw;
        }
    }
}
