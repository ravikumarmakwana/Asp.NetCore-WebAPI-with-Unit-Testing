using Microsoft.EntityFrameworkCore;
using UnitTesting.API.Entities;

namespace UnitTesting.API.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
