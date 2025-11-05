using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Добавление Ocelot
builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle())
    .AddPolly();

// Конфигурация
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//TODO: Добавить Swagger

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("X-Pagination");
    });
});

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();

// Ocelot middleware
await app.UseOcelot();

app.MapGet("/", () => "API Gateway is running!");

app.Run();