namespace MudBarber.Shared.Bookings;

public class BookingDto
{
    public Guid Id { get; init; }

    public Guid BarberId { get; init; }

    public Guid ServiceId { get; init; }

    public DateTimeOffset Start { get; init; }

    public int EstimatedMinutes { get; init; }

    public int? ActualMinutes { get; init; }

    public decimal Price { get; init; }

    // TODO: introduce customer model and use it here.
    public string CustomerName { get; init; } = string.Empty;
}
