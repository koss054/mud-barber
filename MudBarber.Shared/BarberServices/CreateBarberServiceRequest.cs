using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.BarberServices;

// TODO: change string length magic int to const value
public record CreateBarberServiceRequest(
    [property: Required, StringLength(50)] string Name,
    [property: Required] decimal Price,
    [property: Required] int EstimatedMinutes
);