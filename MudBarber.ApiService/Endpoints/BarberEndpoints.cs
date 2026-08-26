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
        group.MapDelete("/{id:guid}", Delete);

        return group;
    }

    private static async Task<Created<BarberDto>> Create(
        CreateBarberRequest request, MudBarberDbContext db, CancellationToken ct)
    {
        var barber = request.ToEntity();

        db.Barbers.Add(barber);
        await db.SaveChangesAsync(ct);

        return TypedResults.Created($"/barbers/{barber.Id}", barber.ToDto());
    }

    private static async Task<Ok<List<BarberDto>>> GetAll(
        MudBarberDbContext db, CancellationToken ct)
    {
        var barbers = await db.Barbers
            .AsNoTracking()
            .ToListAsync(ct);

        return TypedResults.Ok(barbers.Select(b => b.ToDto()).ToList());
    }

    private static async Task<Results<NoContent, NotFound>> Delete(
        Guid id, MudBarberDbContext db, TimeProvider timeProvider, CancellationToken ct)
    {
        // The global query filter already excludes retired barbers,
        // so deleting one twice returns NotFound without an extra predicate.
        var barber = await db.Barbers.FirstOrDefaultAsync(b => b.Id == id, ct);

        if (barber == null)
        {
            return TypedResults.NotFound();
        }

        barber.RetiredAt = timeProvider.GetUtcNow();
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}
