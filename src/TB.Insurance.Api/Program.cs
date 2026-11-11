using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options =>
{
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

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options
        .WithTitle("TB Insurance Calculator API")
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/scalar/v1"))
   .ExcludeFromDescription();

app.Run();

public partial class Program;
