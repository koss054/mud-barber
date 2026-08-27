using MudBarber.ApiService.Data.Entities;
using MudBarber.Shared.Barbers;

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
            RetiredAt = barber.RetiredAt
        };

    public static Barber ToEntity(this CreateBarberRequest request) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim()
        };

    public static void ApplyTo(this UpdateBarberRequest request, Barber barber)
    {
        barber.FirstName = request.FirstName.Trim();
        barber.LastName = request.LastName.Trim();
    }
}
