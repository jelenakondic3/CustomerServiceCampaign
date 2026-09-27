using CustomerServiceCampaign.Api.Data;
using CustomerServiceCampaign.Api.Services;
using Microsoft.EntityFrameworkCore;
using CustomerServiceCampaign.Api.Security;

var builder = WebApplication.CreateBuilder(args);

// Dodajemo podršku za API kontrolere.
builder.Services.AddControllers();

// Registrujemo CampaignDbContext i povezujemo ga sa SQLite bazom.
// Connection string "DefaultConnection" se cita iz appsettings.json.
builder.Services.AddDbContext<CampaignDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrujemo PersonService kao implementaciju IPersonService interfejsa.
// Kada controller zatraži IPersonService, Dependency Injection
// će mu proslediti instancu PersonService-a.
builder.Services.AddScoped<IPersonService, PersonService>();

// Omogucava pronalaženje API endpoint-a.
builder.Services.AddEndpointsApiExplorer();

// Podešavamo Swagger za testiranje zašticenih API endpoint-a.
// API key se unosi preko Authorize dugmeta i šalje kroz X-API-Key HTTP header.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Unesite API key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "X-API-Key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// U Development okruženju ukljucujemo Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Preusmerava HTTP zahteve na HTTPS.
app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();

// Omogucava authorization middleware.
app.UseAuthorization();

// Povezuje naše controllere sa odgovarajucim API rutama.
app.MapControllers();

// Pokrece aplikaciju.
app.Run();