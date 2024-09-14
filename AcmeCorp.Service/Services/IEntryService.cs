using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;
namespace AcmeCorp.Service.Services
{
    public interface IEntryService
    {
        Task<bool> ValidateModelAsync(AddEntryViewModel viewModel);
        Task<bool> ValidateAgeAsync(AddEntryViewModel viewModel);
        Task<(int, string)> ValidateExistingEntries(string serial, int customerId);
        Task SubmitEntryAsync(Entry entry);

        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsyncByEmailAsync(string email);
        Task SubmitCustomerAsync(Customer customer);
        Task<PaginatedListViewModel> GetPaginatedEntriesAsync(int pageNumber, int pageSize);
    }
}
