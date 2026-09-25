using CustomerServiceCampaign.Api.Data;
using CustomerServiceCampaign.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodajemo podršku za API kontrolere.
builder.Services.AddControllers();

// Registrujemo CampaignDbContext i povezujemo ga sa SQLite bazom.
// Connection string "DefaultConnection" se ?ita iz appsettings.json.
builder.Services.AddDbContext<CampaignDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrujemo PersonService u Dependency Injection sistem.
// AddScoped zna?i da se kreira jedna instanca servisa za svaki HTTP zahtev.
builder.Services.AddScoped<PersonService>();

// Omogu?ava pronalaženje API endpoint-a.
builder.Services.AddEndpointsApiExplorer();

// Dodaje Swagger za dokumentovanje i testiranje API-ja.
builder.Services.AddSwaggerGen();

var app = builder.Build();

// U Development okruženju uklju?ujemo Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Preusmerava HTTP zahteve na HTTPS.
app.UseHttpsRedirection();

// Omogu?ava authorization middleware.
app.UseAuthorization();

// Povezuje naše controllere sa odgovaraju?im API rutama.
app.MapControllers();

// Pokre?e aplikaciju.
app.Run();