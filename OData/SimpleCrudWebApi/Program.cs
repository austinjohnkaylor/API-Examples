using API.Examples.OData.SimpleCrudWebApi;
using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

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
        ODataBasicCrudDbContext db = serviceScope.ServiceProvider.GetRequiredService<ODataBasicCrudDbContext>();
        ODataBasicCrudDbHelper.PopulateDatabase(db, 10000);
}

app.UseODataRouteDebug();
app.UseRouting();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }