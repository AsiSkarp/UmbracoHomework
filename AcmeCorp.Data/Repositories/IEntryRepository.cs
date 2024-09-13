using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;

namespace AcmeCorp.Data.Repositories
{
    public interface IEntryRepository
    {
        Task<List<Entry>> GetEntriesBySerialNumberAsync(string serialNumber);
        Task AddEntryAsync(Entry entry);
        Task<(List<ListEntryViewModel> entries, int totalRecords)> GetPaginatedEntriesAsync(int pageNumber, int pageSize);
    }
}
