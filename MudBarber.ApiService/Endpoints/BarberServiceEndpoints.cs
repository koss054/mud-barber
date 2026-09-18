using Microsoft.AspNetCore.Http.HttpResults;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Mapping;
using MudBarber.Shared.BarberServices;

namespace MudBarber.ApiService.Endpoints;

public static class BarberServiceEndpoints
{
    public static RouteGroupBuilder MapBarberServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/barber-services").WithTags("Barber Services");

        group.MapPost("/", Create);

        return group;
    }

    private static async Task<Created<BarberServiceDto>> Create(
        CreateBarberServiceRequest request,
        MudBarberDbContext db,
        CancellationToken ct = default)
    {
        var service = request.ToEntity();

        db.BarberServices.Add(service);
        await db.SaveChangesAsync(ct);

        return TypedResults.Created($"/barber-services/{service.Id}", service.ToDto());
    }
}