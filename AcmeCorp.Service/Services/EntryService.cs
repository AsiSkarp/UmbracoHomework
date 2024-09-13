using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Models;
using AcmeCorp.Data.Repositories;

namespace AcmeCorp.Service.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly ICustomerRepository _customerRepository;

        public EntryService(IEntryRepository entryRepository, ICustomerRepository customerRepository)
        {
            _entryRepository = entryRepository;
            _customerRepository = customerRepository;
        }

        public async Task<bool> ValidateAge(AddEntryViewModel viewModel)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(viewModel.Email);
            var age = DateTime.Now.Year - customer.DateOfBirth.Year;
            if (customer.DateOfBirth > DateTime.Now.AddYears(-age)) age--;
            return age < 18; 
            //return false;
        }

        public async Task<(int, string)> ValidateExistingEntries(string serial, int customerId)
        {
            var existingEntries = await _entryRepository.GetEntriesBySerialNumberAsync(serial);

           
           if (existingEntries.Count == 0)
           {
                return ((int)EntryStatus.FirstTimeEntry, "You have successfully entered once into the prize draw. You may enter the prize draw once more for this serial number");
           }
           else if (existingEntries.Count == 1 && existingEntries[0].CustomerId == customerId)
           {
                return ((int)EntryStatus.SecondTimeEntry, "You have successfully entered twice for the prize draw. You may enter into the prize draw for any other valid serial number(s)");
           }
           else if (existingEntries.Count == 2 || (existingEntries.Count == 1 && existingEntries[0].CustomerId != customerId))
           {
                return ((int)EntryStatus.SerialNumberAlreadyUsed, "This serial number already been entered twice, or is registered to another user. Please input another serial number.");
           }
           return(3, string.Empty);
        }

        public async Task SubmitEntryAsync(Entry entry)
        {
            await _entryRepository.AddEntryAsync(entry);
        }

        public async Task<Customer> GetCustomerAsyncByEmailAsync(string email)
        {
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }

        public async Task SubmitCustomerAsync(Customer customer)
        {
            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task<PaginatedListViewModel> GetPaginatedEntriesAsync(int pageNumber, int pageSize)
        {
            var (entries, totalRecords) = await _entryRepository.GetPaginatedEntriesAsync(pageNumber, pageSize);
            
            return new PaginatedListViewModel
            {
                Entries = entries,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }

    enum EntryStatus
    {
        FirstTimeEntry = 0,
        SecondTimeEntry = 1,
        SerialNumberAlreadyUsed = 2
    }
}
