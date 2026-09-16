namespace MudBarber.ApiService.Data.Entities;

public class Service
{
    public Guid Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int EstimatedMinutes { get; set; }

    public DateTimeOffset? RetiredAt { get; set; }
}
