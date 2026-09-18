using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddValidation();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddOpenApi();

// builder.AddNpgsqlDbContext<MudBarberDbContext>("postgresdb");
builder.AddAzureNpgsqlDbContext<MudBarberDbContext>(
    "postgresdb",
    configureDbContextOptions: o => o.UseSnakeCaseNamingConvention()
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MudBarberDbContext>();
    await db.Database.MigrateAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapBookingEndpoints();
app.MapBarberEndpoints();
app.MapBarberServiceEndpoints();
app.MapDefaultEndpoints();

app.Run();
