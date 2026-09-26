namespace CustomerServiceCampaign.Api.Security
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Proveravamo da li je API key prosleđen u HTTP header-u.
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API key nije prosleđen.");
                return;
            }

            // Učitavamo očekivani API key iz konfiguracije aplikacije.
            var apiKey = _configuration["ApiKey"];

            // Proveravamo da li API key postoji u konfiguraciji
            // i da li odgovara ključu koji je klijent poslao.
            if (string.IsNullOrEmpty(apiKey) || providedApiKey != apiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API key nije ispravan.");
                return;
            }

            // API key je ispravan - zahtev može da nastavi dalje.
            await _next(context);
        }
    }
}