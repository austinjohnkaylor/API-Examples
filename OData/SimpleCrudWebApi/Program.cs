using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ODataConventionModelBuilder modelBuilder = new();
modelBuilder.EnumType<GradeLevel>();
modelBuilder.EntitySet<Student>("Students");
modelBuilder.EntitySet<Teacher>("Teachers");
modelBuilder.EntitySet<Course>("Courses");
modelBuilder.EntitySet<Enrollment>("Enrollments");

builder.Services.AddControllers()
    .AddOData(options => options
        .EnableQueryFeatures(100)
        .AddRouteComponents(
            routePrefix: "odata",
            model: modelBuilder.GetEdmModel()
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
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    SchoolSystemDbContext db = serviceScope.ServiceProvider.GetRequiredService<SchoolSystemDbContext>();
    SchoolSystemDbContextHelper.SeedData(db);
}

app.Run();