using MudBarber.ApiService.Data.Entities;
using MudBarber.Shared.BarberServices;

namespace MudBarber.ApiService.Mapping;

public static class BarberServiceMapping
{
    public static BarberServiceDto ToDto(this BarberService service) =>
        new()
        {
            Id = service.Id,
            Name = service.Name,
            Price = service.Price,
            EstimatedMinutes = service.EstimatedMinutes
        };

    public static BarberService ToEntity(this CreateBarberServiceRequest request) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Price = request.Price,
            EstimatedMinutes = request.EstimatedMinutes
        };

    public static void ApplyTo(
        this UpdateBarberServiceRequest request, 
        BarberService service)
    {
        service.Name = request.Name;
        service.Price = request.Price;
        service.EstimatedMinutes = request.EstimatedMinutes;
    }
}