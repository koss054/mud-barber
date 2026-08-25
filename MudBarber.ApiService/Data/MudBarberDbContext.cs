using Microsoft.EntityFrameworkCore;

namespace MudBarber.ApiService.Data;

public class MudBarberDbContext : DbContext
{
    public MudBarberDbContext(DbContextOptions<MudBarberDbContext> options) : base(options)
    {
    }


}