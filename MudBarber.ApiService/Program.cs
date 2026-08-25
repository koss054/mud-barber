using MudBarber.ApiService.Data;
using MudBarber.ApiService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<MudBarberDbContext>("postgresdb");

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapBookingEndpoints();
app.MapDefaultEndpoints();

app.Run();
