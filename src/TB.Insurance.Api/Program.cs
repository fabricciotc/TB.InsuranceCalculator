using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TB.Insurance.Application.Configuration;
using TB.Insurance.Application.Employees;
using TB.Insurance.Application.Premiums;
using TB.Insurance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<TB.Insurance.Api.Middleware.DomainExceptionHandler>();

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<TB.Insurance.Api.OpenApi.EnumSchemaTransformer>();

    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "TB Insurance Calculator API";
        document.Info.Version = "v1";
        document.Info.Description =
            "Open API that stores employee data and returns a prorated insurance premium. " +
            "The API is intentionally public: there is no authentication or authorization layer.";
        return Task.CompletedTask;
    });
});

builder.Services.AddDbContext<InsuranceDbContext>(options =>
    options.UseInMemoryDatabase("tb-insurance"));

builder.Services.Configure<PremiumSettings>(
    builder.Configuration.GetSection(PremiumSettings.SectionName));

builder.Services.AddScoped<SaveEmployeeHandler>();
builder.Services.AddScoped<GetPremiumHandler>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "TB Insurance Calculator API v1");
    options.DocumentTitle = "TB Insurance Calculator API";
    options.RoutePrefix = "swagger";
});

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();
