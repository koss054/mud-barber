namespace MudBarber.ApiService.Dtos;

public class BarberDto
{
    public Guid Id { get; init; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public decimal Rating { get; set; }

    public bool IsActive { get; set; }
}
