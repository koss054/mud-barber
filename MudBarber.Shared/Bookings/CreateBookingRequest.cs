using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.Bookings;

public record CreateBookingRequest(
    [property: Required] Guid BarberId,
    [property: Required] Guid ServiceId,
    [property: Required] DateTimeOffset Start,
    [property: Required, StringLength(50)] string CustomerName
);
