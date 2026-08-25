using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Dtos;

namespace MudBarber.ApiService.Endpoints;

public static class BookingEndpoints
{
    public static RouteGroupBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bookings").WithTags("Bookings");

        group.MapGet("/", GetAll);

        return group;
    }

    private static async Task<Ok<List<BookingDto>>> GetAll(
        MudBarberDbContext db, CancellationToken ct)
    {
        var bookings = await db.Bookings
            .AsNoTracking()
            .Select(b => new BookingDto
            {
                Id = b.Id,
                BarberId = b.BarberId,
                Start = b.Start,
                DurationMinutes = b.DurationMinutes,
                CustomerName = b.CustomerName
            })
            .ToListAsync(ct);

        return TypedResults.Ok(bookings);
    }
}