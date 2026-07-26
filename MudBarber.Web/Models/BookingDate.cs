namespace MudBarber.Web.Models;

public class BookingDate
{
    public DateOnly Date { get; set; }
    public bool Available { get; set; }
    public IEnumerable<BookingTime> Time { get; set; } = [];
}