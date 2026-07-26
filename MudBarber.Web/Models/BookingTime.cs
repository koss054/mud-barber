namespace MudBarber.Web.Models;

public class BookingTime
{
    public TimeOnly Time { get; set; }
    public bool Available { get; set; }
}