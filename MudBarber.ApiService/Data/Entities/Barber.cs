namespace MudBarber.ApiService.Data.Entities;

public class Barber
{
    public Guid Id { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal Rating { get; set; }

    public DateTimeOffset? RetiredAt { get; set; }

    // TODO: model for available hours depending on barber's upcoming bookings
}