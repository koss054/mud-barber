namespace MudBarber.Web.Models;

/// <summary>
/// Single source of truth for a reservation.
/// Step completion is derived from these fields.
/// </summary>
public class BookingModel
{
    public string? Service { get; set; }
    public string? Barber { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public string? Phone { get; set; }
    public string? Note { get; set; }
}
