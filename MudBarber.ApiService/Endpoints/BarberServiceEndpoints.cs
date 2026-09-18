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
        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPut("/{id:guid}", Update);
        group.MapPost("/{id:guid}/restore", Restore);
        group.MapDelete("/{id:guid}", Delete);

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

    private static async Task<Results<Ok<BarberServiceDto>, NotFound>> GetById(
        Guid id,
        MudBarberDbContext db,
        CancellationToken ct = default)
    {
        var service = await db.BarberServices
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        return service == null
            ? TypedResults.NotFound()
            : TypedResults.Ok(service.ToDto());
    }

    private static async Task<Results<Ok<BarberServiceDto>, NotFound>> Update(
        Guid id,
        UpdateBarberServiceRequest request,
        MudBarberDbContext db,
        CancellationToken ct = default)
    {
        var service = await db.BarberServices
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (service == null)
        {
            return TypedResults.NotFound();
        }

        request.ApplyTo(service);
        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(service.ToDto());
    }

    // TODO: eventually add admin endpoint for fully deleting a record, instead of retiring it
    // TODO: rename this endpoint to Retire when above TODO is implemented
    private static async Task<Results<NoContent, NotFound>> Delete(
        Guid id,
        MudBarberDbContext db,
        TimeProvider timeProvider,
        CancellationToken ct = default)
    {
        // The global query filter already excludes retired barber services.
        var service = await db.BarberServices
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (service == null)
        {
            // Deleting a service twice return NotFound.
            return TypedResults.NotFound();
        }

        service.RetiredAt = timeProvider.GetUtcNow();
        await db.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<BarberServiceDto>, NotFound>> Restore(
        Guid id,
        MudBarberDbContext db,
        CancellationToken ct = default
    )
    {
        var service = await db.BarberServices
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (service == null)
        {
            return TypedResults.NotFound();
        }

        if (service.RetiredAt != null)
        {
            service.RetiredAt = null;
            await db.SaveChangesAsync(ct);
        }

        return TypedResults.Ok(service.ToDto());
    }
}