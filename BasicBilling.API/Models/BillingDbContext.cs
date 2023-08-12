using Microsoft.EntityFrameworkCore;

namespace BasicBilling.API.Models
{
    public class BillingDbContext : DbContext
    {
        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options){}

        // Define DbSet properties for your entities
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bill> Bills { get; set; }
    }
}

