using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.Bookings;

public record UpdateBookingRequest(
    [property: Required] DateTimeOffset Start,
    [property: Required, StringLength(50)] string CustomerName,
    int? ActualMinutes
);
