using AcmeCorp.Data.Db;
using AcmeCorp.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Data.Repositories
{
    public class SerialNumberRepository : ISerialNumberRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SerialNumberRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsAnySerialNumberInDatabaseAsync()
        {
            return await dbContext.SerialNumbers.AnyAsync();
        }

        public async Task<bool> IsSerialNumberInDatabaseAsync(string serialNumber)
        {
            var serial = await dbContext.SerialNumbers
            .FirstOrDefaultAsync(s => s.Serial == serialNumber);
            return serial != null;
        }


        public async Task SerialNumberAddAsync(SerialNumber serialNumber)
        {
            dbContext.SerialNumbers.Add(serialNumber);
        }

        public async Task SerialNumbersSaveChangesAsync()
        {
            dbContext.SaveChanges();
        }
    }
}
