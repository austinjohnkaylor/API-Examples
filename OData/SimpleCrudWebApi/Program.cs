using API.Examples.OData.SimpleCrudWebApi;
using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(options => options
        .EnableQueryFeatures(100)
        .AddRouteComponents(
            routePrefix: "odata",
            model: ODataEdmModelBuilder.GetEdmModel()
            )
    );

builder.Services.AddDbContext<SchoolSystemDbContext>(options =>
    options.UseInMemoryDatabase("SchoolSystemDb"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed database
using (IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    SchoolSystemDbContext db = serviceScope.ServiceProvider.GetRequiredService<SchoolSystemDbContext>();
    SchoolSystemDbContextHelper.SeedData(db);
}

app.Run();