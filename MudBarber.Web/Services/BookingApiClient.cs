using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MudBarber.Shared.Bookings;
using MudBarber.Shared.Paging;

namespace MudBarber.Web.Services;

public class BookingApiClient(HttpClient httpClient)
{
    public async Task<CreateBookingResult> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("/bookings", request, ct);

        // Create rejects a retired barber or service with a 400, so the field errors
        // are read out instead of being thrown away by EnsureSuccessStatusCode.
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(ct);

            return new CreateBookingResult(
                null,
                problem?.Errors.AsReadOnly() ?? ReadOnlyDictionary<string, string[]>.Empty);
        }

        response.EnsureSuccessStatusCode();

        var booking = await response.Content.ReadFromJsonAsync<BookingDto>(ct);

        return new CreateBookingResult(booking, ReadOnlyDictionary<string, string[]>.Empty);
    }

    public async Task<PagedResult<BookingDto>> GetBookingsAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var url = $"/bookings?page={page}&pageSize={pageSize}";

        return await httpClient.GetFromJsonAsync<PagedResult<BookingDto>>(url, ct)
            ?? new PagedResult<BookingDto> { Page = page, PageSize = pageSize };
    }

    public async Task<BookingDto?> GetBookingAsync(
        Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync($"/bookings/{id}", ct);

        return await ReadBookingAsync(response, ct);
    }

    public async Task<BookingDto?> UpdateBookingAsync(
        Guid id, UpdateBookingRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/bookings/{id}", request, ct);

        return await ReadBookingAsync(response, ct);
    }

    private static async Task<BookingDto?> ReadBookingAsync(
        HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BookingDto>(ct);
    }
}
