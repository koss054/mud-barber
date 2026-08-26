using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Dtos.Barbers;
using MudBarber.ApiService.Mapping;

namespace MudBarber.ApiService.Endpoints;

public static class BarberEndpoints
{
    public static RouteGroupBuilder MapBarberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/barbers").WithTags("Barbers");

        group.MapPost("/", Create);
        group.MapGet("/", GetAll);

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
}
