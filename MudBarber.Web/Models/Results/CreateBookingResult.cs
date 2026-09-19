using MudBarber.Shared.Bookings;

// TODO: decide if this is the actual directory for this record
namespace MudBarber.Web.Models.Results;

public record CreateBookingResult(
    BookingDto? Booking,
    IReadOnlyDictionary<string, string[]> Errors)
{
    public bool Succeeded => Booking is not null;
}
