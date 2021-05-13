using Microsoft.EntityFrameworkCore;
using UnitTesting.Entities;

namespace UnitTesting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
