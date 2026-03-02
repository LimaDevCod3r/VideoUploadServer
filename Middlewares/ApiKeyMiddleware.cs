namespace VideoUploadServer.Middlewares;

public class ApiKeyMiddleware
{

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;


    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }


    public async Task InvokeAsync(HttpContext context)
    {


        // 1 Pegar o Header "x-api-key" da requisição
        var apiKeyHeader = context.Request.Headers["x-api-key"];
        var secretKey = _configuration["ApiSecurity:Apikey"];

        if (apiKeyHeader == secretKey)
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key inválida");
            return;
        }

    }
}