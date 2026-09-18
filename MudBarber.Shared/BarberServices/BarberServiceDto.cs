namespace MudBarber.Shared.BarberServices;

public class BarberServiceDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int EstimatedMinutes { get; init; }
}
