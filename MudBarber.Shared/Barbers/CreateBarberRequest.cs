using System.ComponentModel.DataAnnotations;

namespace MudBarber.Shared.Barbers;

public record CreateBarberRequest(
    [property: Required, StringLength(50)] string FirstName,
    [property: Required, StringLength(50)] string LastName);
