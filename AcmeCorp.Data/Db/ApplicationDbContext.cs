using Microsoft.EntityFrameworkCore;
using AcmeCorp.Data.Models.Entities;

namespace AcmeCorp.Data.Db
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
