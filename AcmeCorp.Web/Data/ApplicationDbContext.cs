using AcmeCorp.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Web.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Customer> Customers{ get; set; }
        public DbSet<SerialNumber> SerialNumbers{ get; set; }
        public DbSet<Entry> Entries{ get; set; }
    }
}
