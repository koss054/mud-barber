namespace MudBarber.ApiService.Data.Entities;

public class Booking
{
    public Guid Id { get; init; }

    public Guid BarberId { get; set; }

    public DateTimeOffset Start { get; set; }

    public int DurationMinutes { get; set; }

    // TODO: introduce customer model and use it here.
    public string CustomerName { get; set; } = string.Empty;
}