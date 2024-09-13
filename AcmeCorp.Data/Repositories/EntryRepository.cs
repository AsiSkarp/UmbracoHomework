using AcmeCorp.Data.Db;
using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Data.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public EntryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Entry>> GetEntriesBySerialNumberAsync(string serialNumber)
        {
            return await dbContext.Entries
                .Where(e => e.SerialNumber == serialNumber)
                .ToListAsync();
        }

        public async Task AddEntryAsync(Entry entry)
        {
            await dbContext.Entries.AddAsync(entry);
            await dbContext.SaveChangesAsync();
        }

        public async Task<(List<ListEntryViewModel> entries, int totalRecords)> GetPaginatedEntriesAsync(int pageNumber, int pageSize)
        {
            var query = from entry in dbContext.Entries
                        join customer in dbContext.Customers
                        on entry.CustomerId equals customer.Id
                        select new ListEntryViewModel
                        {
                            FullName = customer.FirstName + " " + customer.LastName,
                            Email = customer.Email,
                            SerialNumber = entry.SerialNumber,
                            EntryTime = entry.EntryTime
                        };

            int totalRecords = await query.CountAsync();

            var entries = await query
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();
            return (entries, totalRecords);
        }
    }
}
