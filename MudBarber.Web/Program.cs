using MudBarber.Web.Components;
using MudBarber.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();


// "https+http://" prefers HTTPS over HTTP.
// "apiservice" is the AppHost resource name, resolved by service discovery.
static void UseApiService(HttpClient client) =>
    client.BaseAddress = new("https+http://apiservice");

builder.Services.AddHttpClient<BarberApiClient>(UseApiService);
builder.Services.AddHttpClient<BarberServiceApiClient>(UseApiService);
builder.Services.AddHttpClient<BookingApiClient>(UseApiService);

builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
