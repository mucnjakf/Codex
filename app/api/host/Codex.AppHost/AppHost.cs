IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint("tcp", endpoint => endpoint.Port = 59287);

IResourceBuilder<PostgresDatabaseResource> database = postgres
    .AddDatabase("codex-db");

IResourceBuilder<ProjectResource> api = builder
    .AddProject<Projects.Codex_Api>("codex-api")
    .WithUrlForEndpoint("http", url => url.Url += "/scalar")
    .WithUrlForEndpoint("https", url => url.Url += "/scalar")
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();