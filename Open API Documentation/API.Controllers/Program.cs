using System.Reflection;
using API.Controllers;
using API.Controllers.EntityFramework;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddApiVersioning(
        options =>
        {
            // reporting api versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;

            options.Policies.Sunset( 0.9 )
                .Effective( DateTimeOffset.Now.AddDays( 60 ) )
                .Link( "policy.html" )
                .Title( "Versioning Policy" )
                .Type( "text/html" );
        } )
    .AddMvc()
    .AddApiExplorer(
        options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        } );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        // add a custom operation filter which sets default values
        options.OperationFilter<SwaggerDefaultValues>();
        options.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
        options.EnableAnnotations(); // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#swashbuckleaspnetcoreannotations
        string fileName = typeof( Program ).Assembly.GetName().Name + ".xml";
        string filePath = Path.Combine( AppContext.BaseDirectory, fileName);

        // integrate xml comments
        options.IncludeXmlComments(filePath);
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
    var descriptions = app.DescribeApiVersions();

    // build a swagger endpoint for each discovered API version
    foreach ( ApiVersionDescription description in descriptions )
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        string name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint( url, name );
    }

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
