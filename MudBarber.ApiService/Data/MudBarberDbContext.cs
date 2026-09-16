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

    public DbSet<Service> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Retired barbers are excluded from every query by default.
        // Admin reads opt out explicitly with IgnoreQueryFilters().
        // TODO: check how the Service approach below (with a named query filter) would work for this
        modelBuilder.Entity<Barber>()
            .HasQueryFilter(b => b.RetiredAt == null);

        modelBuilder.Entity<Service>()
            .HasQueryFilter("NotRetired", s => s.RetiredAt == null);

        modelBuilder.Entity<Booking>(booking =>
        {
            booking.HasOne(b => b.Barber)
                .WithMany()
                .HasForeignKey(b => b.BarberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            booking.HasOne(b => b.Service)
                .WithMany()
                .HasForeignKey(b => b.ServiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}