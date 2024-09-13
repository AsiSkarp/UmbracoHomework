using AcmeCorp.Data.Db;
using AcmeCorp.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace AcmeCorp.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
        }

    }
}
