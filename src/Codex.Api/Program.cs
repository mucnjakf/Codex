using System.Reflection;
using Codex.Api.Extensions;
using Codex.Application;
using Codex.Infrastructure;
using Codex.ServiceDefaults;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddApplicationModule(builder.Configuration);
builder.Services.AddInfrastructureModule(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
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

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.Run();