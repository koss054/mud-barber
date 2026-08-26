using Microsoft.EntityFrameworkCore;
using MudBarber.ApiService.Data.Entities;

namespace MudBarber.ApiService.Data;

public class MudBarberDbContext : DbContext
{
    public MudBarberDbContext(DbContextOptions<MudBarberDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Barber> Barbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Retired barbers are excluded from every query by default.
        // Admin reads opt out explicitly with IgnoreQueryFilters().
        modelBuilder.Entity<Barber>()
            .HasQueryFilter(b => b.RetiredAt == null);
    }
}