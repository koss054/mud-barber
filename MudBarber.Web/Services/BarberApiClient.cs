using MudBarber.Shared.Barbers;

namespace MudBarber.Web.Services;

public class BarberApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<BarberDto>> GetBarbersAsync(CancellationToken ct = default) =>
        await httpClient.GetFromJsonAsync<List<BarberDto>>("/barbers", ct) ?? [];
}
