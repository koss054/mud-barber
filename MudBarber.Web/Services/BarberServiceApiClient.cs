using System.Net;
using MudBarber.Shared.BarberServices;
using MudBarber.Shared.Paging;

namespace MudBarber.Web.Services;

public class BarberServiceApiClient(HttpClient httpClient)
{
    public async Task<BarberServiceDto?> CreateBarberServiceAsync(
        CreateBarberServiceRequest request,
        CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("/barber-services", request, ct);

        return await ReadBarberAsync(response, ct);
    }

    public async Task<PagedResult<BarberServiceDto>> GetBarberServicesAsync(
        int page = 1,
        int pageSize = 4,
        bool includeRetired = false,
        CancellationToken ct = default)
    {
        var url = $"/barber-services?page={page}&pageSize={pageSize}&includeRetired={(includeRetired ? "true" : "false")}";

        return await httpClient.GetFromJsonAsync<PagedResult<BarberServiceDto>>(url, ct)
            ?? new PagedResult<BarberServiceDto> { Page = page, PageSize = pageSize };
    }

    private static async Task<BarberServiceDto?> ReadBarberAsync(
        HttpResponseMessage response,
        CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }   

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BarberServiceDto>(ct);
    }
}