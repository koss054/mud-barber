using MudBarber.Shared.Bookings;

namespace MudBarber.Web.Services;

public record CreateBookingResult(
    BookingDto? Booking,
    IReadOnlyDictionary<string, string[]> Errors)
{
    public bool Succeeded => Booking is not null;
}
