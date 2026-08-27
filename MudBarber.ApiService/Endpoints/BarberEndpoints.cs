using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Mapping;
using MudBarber.Shared.Barbers;

namespace MudBarber.ApiService.Endpoints;

public static class BarberEndpoints
{
    public static RouteGroupBuilder MapBarberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/barbers").WithTags("Barbers");

        group.MapPost("/", Create);
        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPut("/{id:guid}", Update);
        group.MapPost("/{id:guid}/restore", Restore);
        group.MapDelete("/{id:guid}", Delete);

        return group;
    }

    private static async Task<Created<BarberDto>> Create(
        CreateBarberRequest request, MudBarberDbContext db, CancellationToken ct = default)
    {
        var barber = request.ToEntity();

        db.Barbers.Add(barber);
        await db.SaveChangesAsync(ct);

        return TypedResults.Created($"/barbers/{barber.Id}", barber.ToDto());
    }

    private static async Task<Ok<List<BarberDto>>> GetAll(
        MudBarberDbContext db, bool includeRetired = false, CancellationToken ct = default)
    {
        var query = db.Barbers.AsNoTracking();

        if (includeRetired)
        {
            query = query.IgnoreQueryFilters();
        }

        var barbers = await query.ToListAsync(ct);

        return TypedResults.Ok(barbers.Select(b => b.ToDto()).ToList());
    }

    private static async Task<Results<Ok<BarberDto>, NotFound>> GetById(
        Guid id, MudBarberDbContext db, CancellationToken ct = default)
    {
        var barber = await db.Barbers
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        return barber == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(barber.ToDto());
    }

    private static async Task<Results<Ok<BarberDto>, NotFound>> Update(
        Guid id, UpdateBarberRequest request, MudBarberDbContext db, CancellationToken ct = default)
    {
        var barber = await db.Barbers.FirstOrDefaultAsync(b => b.Id == id, ct);

        if (barber == null)
        {
            return TypedResults.NotFound();
        }

        request.ApplyTo(barber);
        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(barber.ToDto());
    }

    private static async Task<Results<NoContent, NotFound>> Delete(
        Guid id, MudBarberDbContext db, TimeProvider timeProvider, CancellationToken ct = default)
    {
        // The global query filter already excludes retired barbers.
        var barber = await db.Barbers.FirstOrDefaultAsync(b => b.Id == id, ct);

        if (barber == null)
        {
            // Deleting one twice returns NotFound.
            return TypedResults.NotFound();
        }

        barber.RetiredAt = timeProvider.GetUtcNow();
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<BarberDto>, NotFound>> Restore(
        Guid id, MudBarberDbContext db, CancellationToken ct = default)
    {
        var barber = await db.Barbers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        if (barber == null)
        {
            return TypedResults.NotFound();
        }

        if (barber.RetiredAt != null)
        {
            barber.RetiredAt = null;
            await db.SaveChangesAsync(ct);
        }

        return TypedResults.Ok(barber.ToDto());
    }
}
