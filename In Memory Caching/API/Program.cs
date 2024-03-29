using InMemoryCaching.API;
using InMemoryCaching.API.EntityFramework;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PersonContext>(opts => opts.UseInMemoryDatabase("People"));
builder.Services.AddScoped<DatabaseInitializer>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedDatabase(1000);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace InMemoryCaching.API
{
    public partial class Program{}
}
