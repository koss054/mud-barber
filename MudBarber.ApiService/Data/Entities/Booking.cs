namespace MudBarber.ApiService.Data.Entities;

public class Booking
{
    public Guid Id { get; init; }

    public Guid BarberId { get; set; }
    public Barber Barber { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public BarberService Service { get; set; } = null!;

    public DateTimeOffset Start { get; set; }

    public int EstimatedMinutes { get; set; }
    public int? ActualMinutes { get; set; }

    public decimal Price { get; set; }

    // TODO: introduce customer model and use it here.
    public string CustomerName { get; set; } = string.Empty;
}