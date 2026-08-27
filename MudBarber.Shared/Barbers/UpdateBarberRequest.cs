using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.Barbers;

public record UpdateBarberRequest(
    [property: Required, StringLength(50)] string FirstName,
    [property: Required, StringLength(50)] string LastName);
