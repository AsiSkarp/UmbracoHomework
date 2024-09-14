using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Models;
using AcmeCorp.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Globalization;

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

        public async Task<bool> ValidateAgeAsync(AddEntryViewModel viewModel)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(viewModel.Email);
            var age = DateTime.Now.Year - customer.DateOfBirth.Year;
            if (customer.DateOfBirth > DateTime.Now.AddYears(-age)) age--;
            return age < 18; 
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

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _customerRepository.GetCustomersAsync();
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

        public async Task<bool> ValidateModelAsync(AddEntryViewModel viewModel)
        {
            if (isValidString(viewModel.FirstName) && 
                isValidString(viewModel.LastName) && 
                isValidEmail(viewModel.Email) && 
                isValidSerial(viewModel.Serial) && 
                isValidAge(viewModel.DateOfBirth))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isValidString(string validationString)
        {
            switch (validationString)
            {
                case string a when string.IsNullOrEmpty(a): return false;
                case string a when a.Length > 50: return false;
                case string a when a.Any(char.IsDigit): return false;
            }
            return true;
        }

        private bool isValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool isValidSerial(string serial)
        {
            var serialPattern = "^ACME-([A-Z0-9]{3}-){2}[A-Z0-9]{3}$";
            return Regex.IsMatch(serial, serialPattern);
        }

        private bool isValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Now.AddYears(-age)) age--;
            return age > 18;
        }
    }

    enum EntryStatus
    {
        FirstTimeEntry = 0,
        SecondTimeEntry = 1,
        SerialNumberAlreadyUsed = 2
    }
}
