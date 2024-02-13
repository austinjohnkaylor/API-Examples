using System.Reflection;
using API.Controllers;
using API.Controllers.EntityFramework;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "SimpleStore API", Version = "v1" });
    options.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
    options.EnableAnnotations(); // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#swashbuckleaspnetcoreannotations
    // Enable XML comments to show up for the API
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<SimpleStoreDbContext>(opts => opts.UseInMemoryDatabase("SimpleStore"));
builder.Services.AddScoped<DatabaseInitializer>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles(); // necessary to customize the way swagger looks
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectStylesheet("/swagger-ui/custom.css"); // necessary to customize the way swagger looks
});
// Add ReDoc UI to interact with the document
// Available at: http://localhost:<port>/redoc
app.UseReDoc(options =>
{
    options.Path = "/redoc"; // serve the redoc UI at the /redoc endpoint https://localhost:<port>/redoc
});
app.SeedDatabase(1000,5, 5);

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
