using System.ComponentModel.DataAnnotations;

namespace MudBarber.ApiService.Dtos.Barbers;

public record CreateBarberRequest(
    [property: Required, StringLength(50)] string FirstName,
    [property: Required, StringLength(50)] string LastName);
