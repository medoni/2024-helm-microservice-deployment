namespace PizzaService.Aws.Services.AspNet;

internal class StripPathMiddleware(
    RequestDelegate next,
    ILogger<StripPathMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        //logger.LogTrace("Processing request '{request}', ...", context.Request);
        logger.LogDebug("Processing request path:'{requestPath}', X-Forwarded-Prefix:'{forwardPrefix}' ...", context.Request.Path, GetHeader(context, "X-Forwarded-Prefix"));

        var originalPath = context.Request.Path.Value;
        if (originalPath?.StartsWith("/prod/api/") == false)
        {
            context.Request.Path = originalPath.Substring("/prod/api".Length);
            logger.LogInformation("Rewritten path from '{fromPath}' to '{toPath}'", originalPath, context.Request.Path);
        }

        logger.LogDebug("Processing request {method}, '{path}' ...", context.Request.Method, context.Request.Path.Value);
        await next(context);
    }

    private static string? GetHeader(HttpContext context, string headerKey)
    {
        if (!context.Request.Headers.TryGetValue(headerKey, out var headerValues)) return null;
        return headerValues.FirstOrDefault();
    }
}
