using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Mapping;
using MudBarber.Shared.Bookings;
using MudBarber.Shared.Paging;

namespace MudBarber.ApiService.Endpoints;

public static class BookingEndpoints
{
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 50;

    public static RouteGroupBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bookings").WithTags("Bookings");

        group.MapPost("/", Create);
        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPut("/{id:guid}", Update);

        return group;
    }

    private static async Task<Results<Created<BookingDto>, ValidationProblem>> Create(
        CreateBookingRequest request, MudBarberDbContext db, CancellationToken ct = default)
    {
        // Both lookups run through the retired query filters, so a retired barber
        // or service reads as "doesn't exist" and the booking is rejected.
        var barberExists = await db.Barbers.AnyAsync(b => b.Id == request.BarberId, ct);
        var service = await db.BarberServices.FirstOrDefaultAsync(s => s.Id == request.ServiceId, ct);

        var errors = new Dictionary<string, string[]>();

        if (!barberExists)
        {
            errors[nameof(request.BarberId)] = ["No active barber with this id."];
        }

        if (service is null)
        {
            errors[nameof(request.ServiceId)] = ["No active service with this id."];
        }

        if (errors.Count > 0 || service is null)
        {
            return TypedResults.ValidationProblem(errors);
        }

        var booking = request.ToEntity(service);

        db.Bookings.Add(booking);
        await db.SaveChangesAsync(ct);

        return TypedResults.Created($"/bookings/{booking.Id}", booking.ToDto());
    }

    private static async Task<Ok<PagedResult<BookingDto>>> GetAll(
        MudBarberDbContext db,
        int page = 1,
        int pageSize = DefaultPageSize,
        CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = db.Bookings.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var bookings = await query
            .OrderBy(b => b.Start)
            .ThenBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return TypedResults.Ok(new PagedResult<BookingDto>
        {
            Items = [.. bookings.Select(b => b.ToDto())],
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    private static async Task<Results<Ok<BookingDto>, NotFound>> GetById(
        Guid id, MudBarberDbContext db, CancellationToken ct = default)
    {
        var booking = await db.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        return booking == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(booking.ToDto());
    }

    private static async Task<Results<Ok<BookingDto>, NotFound>> Update(
        Guid id, UpdateBookingRequest request, MudBarberDbContext db, CancellationToken ct = default)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);

        if (booking == null)
        {
            return TypedResults.NotFound();
        }

        request.ApplyTo(booking);
        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(booking.ToDto());
    }
}
