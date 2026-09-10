var builder = DistributedApplication.CreateBuilder(args);

/*
TODO: adjust for local development so I don't need to uncomment this locally
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgWeb();

var postgresdb = postgres.AddDatabase("postgresdb");
*/

var postgres = builder
    .AddAzurePostgresFlexibleServer("postgres")
    .RunAsContainer();

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
