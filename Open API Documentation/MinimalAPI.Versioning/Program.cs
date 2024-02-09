using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MinimalAPI.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

// Add services to the container.
services.AddEndpointsApiExplorer();
services.AddApiVersioning(
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
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSwaggerGen( options => options.OperationFilter<SwaggerDefaultValues>() );

WebApplication app = builder.Build();

// Have to define the API version set in order to use it on endpoints
ApiVersionSet versionSet = app.NewApiVersionSet()
    .HasApiVersion(1.0)
    .HasApiVersion(2.0)
    .HasApiVersion(3.0)
    .ReportApiVersions()
    .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Endpoints

app.MapGet("/", () => "Hello World!")
    .WithName("Hello")
    .WithDescription("Says hello.")
    .Produces<string>()
    .WithTags("Greeting")
    .WithApiVersionSet(versionSet)
    .HasApiVersion(1.0);


#endregion

app.Run();
