using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data;
using MudBarber.ApiService.Mapping;
using MudBarber.Shared.BarberServices;
using MudBarber.Shared.Paging;

namespace MudBarber.ApiService.Endpoints;

public static class BarberServiceEndpoints
{
    // TODO: move these integers to a const class or something like that
    private const int DefaultPageSize = 4;
    private const int MaxPageSize = 50;

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

    private static async Task<Ok<PagedResult<BarberServiceDto>>> GetAll(
        MudBarberDbContext db,
        bool includeRetired = false,
        int page = 1,
        int pageSize = DefaultPageSize,
        CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var query = db.BarberServices.AsNoTracking();

        if (includeRetired)
        {
            query = query.IgnoreQueryFilters(["NotRetired"]);
        }

        var totalCount = await query.CountAsync(ct);

        var services = await query
            .OrderBy(s => s.Name)
            .ThenBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return TypedResults.Ok(new PagedResult<BarberServiceDto>
        {
            Items = [.. services.Select(s => s.ToDto())],
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }
}