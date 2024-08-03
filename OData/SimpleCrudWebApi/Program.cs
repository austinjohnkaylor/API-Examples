using API.Examples.OData.SimpleCrudWebApi;
using API.Examples.OData.SimpleCrudWebApi.Configuration;
using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddOData(options => options
        .EnableQueryFeatures(100)
        .AddRouteComponents(
            routePrefix: "odata",
            model: CustomerEdmModelBuilder.GetEdmModel()
            )
    );

builder.Services.AddDbContext<ODataBasicCrudDbContext>(options =>
    options.UseInMemoryDatabase("ODataBasicCrudDb"));

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
    DatabaseOptions options = serviceScope.ServiceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    if (options.SeedData)
    {
        ODataBasicCrudDbContext db = serviceScope.ServiceProvider.GetRequiredService<ODataBasicCrudDbContext>();
        ODataBasicCrudDbHelper.PopulateDatabase(db, options.CustomersToGenerate);
    }
}

app.Run();