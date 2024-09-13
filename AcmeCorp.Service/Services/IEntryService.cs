using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;
namespace AcmeCorp.Service.Services
{
    public interface IEntryService
    {
        Task<bool> ValidateAge(AddEntryViewModel viewModel);
        Task<(int, string)> ValidateExistingEntries(string serial, int customerId);
        Task SubmitEntryAsync(Entry entry);
        Task<Customer> GetCustomerAsyncByEmailAsync(string email);
        Task SubmitCustomerAsync(Customer customer);
        Task<PaginatedListViewModel> GetPaginatedEntriesAsync(int pageNumber, int pageSize);
    }
}
