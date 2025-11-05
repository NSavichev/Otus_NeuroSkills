using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Therapy_Service.Therapy.DataAccess.Database;
using Therapy_Service.Therapy.Host.Models;
using Therapy_Service.Therapy.Core.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConStr")));
builder.Services.AddControllers();
builder.Services.AddScoped<BaseRepository<Exercise>>();
builder.Services.AddScoped<BaseRepository<Progress>>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TherapyService", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TherapyService V1");
    });
}

app.Run();