using MudBarber.ApiService.Data.Entities;
using MudBarber.ApiService.Dtos.Barbers;

namespace MudBarber.ApiService.Mapping;

public static class BarberMapping
{
    public static BarberDto ToDto(this Barber barber) =>
        new()
        {
            Id = barber.Id,
            FirstName = barber.FirstName,
            LastName = barber.LastName,
            Rating = barber.Rating,
            IsActive = barber.IsActive
        };

    public static Barber ToEntity(this CreateBarberRequest request) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            IsActive = true
        };
}
