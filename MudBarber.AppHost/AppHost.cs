var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddAzurePostgresFlexibleServer("postgres")
    .RunAsContainer(configure => configure
        .WithLifetime(ContainerLifetime.Persistent)
        // Pinned so `dotnet ef` can reach the container outside Aspire.
        .WithHostPort(5432)
        .WithDataVolume(isReadOnly: false)
        .WithPgWeb()
    );

var postgresdb = postgres.AddDatabase("postgresdb");

// TODO: finish setting this up to test AppInsights in Azure
// var appInsightsConnString = builder.Configuration["AppInsightsConnString"];

var apiService = builder.AddProject<Projects.MudBarber_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddProject<Projects.MudBarber_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
