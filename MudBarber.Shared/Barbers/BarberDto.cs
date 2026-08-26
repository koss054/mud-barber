namespace MudBarber.Shared.Barbers;

public class BarberDto
{
    public Guid Id { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal Rating { get; set; }

    public DateTimeOffset? RetiredAt { get; set; }
}
