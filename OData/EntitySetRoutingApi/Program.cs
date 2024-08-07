using EntitySetRoutingApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ODataConventionModelBuilder modelBuilder = new();
modelBuilder.EntitySet<Shape>("Shapes"); // Implicitly, Shape, Rectangle and Circle get included in the Edm model as entity types

builder.Services.AddControllers().AddOData(
    options => options.EnableQueryFeatures(null).AddRouteComponents(
        routePrefix: "odata",
        model: modelBuilder.GetEdmModel()));

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
app.UseODataRouteDebug();
app.UseRouting();
app.UseAuthorization();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints => endpoints.MapControllers());
#pragma warning restore ASP0014

app.Run();

public partial class Program {}