using MudBarber.ApiService.Data.Entities;
using MudBarber.Shared.Bookings;

namespace MudBarber.ApiService.Mapping;

public static class BookingMapping
{
    public static BookingDto ToDto(this Booking booking) =>
        new()
        {
            Id = booking.Id,
            BarberId = booking.BarberId,
            ServiceId = booking.ServiceId,
            Start = booking.Start,
            EstimatedMinutes = booking.EstimatedMinutes,
            ActualMinutes = booking.ActualMinutes,
            Price = booking.Price,
            CustomerName = booking.CustomerName
        };

    // Price and duration are copied off the service rather than read through it,
    // so repricing a service later doesn't rewrite what past bookings cost.
    public static Booking ToEntity(this CreateBookingRequest request, BarberService service) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            BarberId = request.BarberId,
            ServiceId = service.Id,
            Start = request.Start,
            EstimatedMinutes = service.EstimatedMinutes,
            Price = service.Price,
            CustomerName = request.CustomerName.Trim()
        };

    public static void ApplyTo(this UpdateBookingRequest request, Booking booking)
    {
        booking.Start = request.Start;
        booking.CustomerName = request.CustomerName.Trim();
        booking.ActualMinutes = request.ActualMinutes;
    }
}
