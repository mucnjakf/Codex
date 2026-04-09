using System.Reflection;
using Codex.Api.Exceptions;
using Codex.Api.Extensions;
using Codex.Application;
using Codex.Infrastructure;
using Codex.ServiceDefaults;
using FluentValidation;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();

// API docs
builder.Services.AddOpenApi();

// Error handling
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<RequestValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Minimal API
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Modules
builder.Services.AddApplicationModule(builder.Configuration);
builder.Services.AddInfrastructureModule(builder.Configuration);

// --------------------------------------------------------------------------------------------------------------------

WebApplication app = builder.Build();

// Minimal API
app.MapEndpoints();

// API docs
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Codex API")
        .WithClassicLayout()
        .WithTheme(ScalarTheme.Alternate)
        .ExpandAllTags()
        .SortOperationsByMethod();

    options.HideClientButton = true;
});

// Database
app.ApplyMigrations();

// API
app.UseHttpsRedirection();

// Error handling
app.UseExceptionHandler();

// Aspire
app.MapDefaultEndpoints();

app.Run();