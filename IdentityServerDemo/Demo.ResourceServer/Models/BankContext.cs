using Microsoft.EntityFrameworkCore;

namespace Demo.ResourceServer.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) :base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
