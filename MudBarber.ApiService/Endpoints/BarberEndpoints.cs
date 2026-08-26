using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Dtos;

namespace MudBarber.ApiService.Endpoints;

public static class BarberEndpoints
{
    public static RouteGroupBuilder MapBarberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/barbers").WithTags("Barbers");

        group.MapGet("/", GetAll);

        return group;
    }

    private static async Task<Ok<List<BarberDto>>> GetAll(
        MudBarberDbContext db, CancellationToken ct)
    {
        var barbers = await db.Barbers
            .AsNoTracking()
            .Select(b => new BarberDto
            {
                Id = b.Id,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Rating = b.Rating,
                IsActive = b.IsActive
            })
            .ToListAsync(ct);

        return TypedResults.Ok(barbers);
    }
}
