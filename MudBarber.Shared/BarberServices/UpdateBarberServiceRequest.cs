using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.BarberServices;

public record UpdateBarberServiceRequest(
    [property: Required, StringLength(50)] string Name,
    [property: Required] decimal Price,
    [property: Required] int EstimatedMinutes
);