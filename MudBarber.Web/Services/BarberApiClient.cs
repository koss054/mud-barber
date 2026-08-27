using System.Net;
using MudBarber.Shared.Barbers;
using MudBarber.Shared.Paging;

namespace MudBarber.Web.Services;

public class BarberApiClient(HttpClient httpClient)
{
    public async Task<BarberDto?> CreateBarberAsync(
        CreateBarberRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("/barbers", request, ct);

        return await ReadBarberAsync(response, ct);
    }

    public async Task<PagedResult<BarberDto>> GetBarbersAsync(
        int page = 1,
        int pageSize = 12,
        bool includeRetired = false,
        CancellationToken ct = default)
    {
        var url = $"/barbers?page={page}&pageSize={pageSize}&includeRetired={(includeRetired ? "true" : "false")}";

        return await httpClient.GetFromJsonAsync<PagedResult<BarberDto>>(url, ct)
            ?? new PagedResult<BarberDto> { Page = page, PageSize = pageSize };
    }

    public async Task<BarberDto?> GetBarberAsync(
        Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"/barbers/{id}", ct);

        return await ReadBarberAsync(response, ct);
    }

    public async Task<BarberDto?> UpdateBarberAsync(
        Guid id, UpdateBarberRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/barbers/{id}", request, ct);

        return await ReadBarberAsync(response, ct);
    }

    public async Task<bool> RetireBarberAsync(
        Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.DeleteAsync($"/barbers/{id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }

    public async Task<BarberDto?> RestoreBarberAsync(
        Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsync($"/barbers/{id}/restore", content: null, ct);

        return await ReadBarberAsync(response, ct);
    }

    private static async Task<BarberDto?> ReadBarberAsync(
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BarberDto>(ct);
    }
}
