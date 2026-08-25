using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data.Entities;

namespace MudBarber.ApiService.Data;

public class MudBarberDbContext : DbContext
{
    public MudBarberDbContext(DbContextOptions<MudBarberDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }
}