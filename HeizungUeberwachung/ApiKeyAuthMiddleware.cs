namespace HeizungUeberwachung;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly IConfiguration configuration;

    public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        this.next = next;
        this.configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? apiKey = context.Request.Headers["ApiKey"].FirstOrDefault();
        if (apiKey is null || apiKey != configuration["ApiKey"])
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        } 
        else
        {
            await next(context);
        }
    }
}
