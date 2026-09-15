namespace MudBarber.Shared.BarberServices;

public class ServicesDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    // TODO: decide if this is a good name for this property
    public int Minutes { get; init; }
}
