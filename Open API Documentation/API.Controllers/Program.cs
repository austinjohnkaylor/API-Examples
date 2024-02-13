using API.Controllers;
using API.Controllers.EntityFramework;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SimpleStoreDbContext>(opts => opts.UseInMemoryDatabase("SimpleStore"));
builder.Services.AddScoped<DatabaseInitializer>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.SeedDatabase(1000,5, 5);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
