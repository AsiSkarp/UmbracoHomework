using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AcmeCorp.Service.Services;

namespace AcmeCorp.Web.Controllers
{
    public class EntriesController : Controller
    {

        private readonly IEntryService _entryService;
        private readonly ISerialService _serialService;

        public EntriesController(IEntryService entryService, ISerialService serialService)
        {
            _entryService = entryService;
            _serialService = serialService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await _serialService.OccupySerialNumberDatabase();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEntryViewModel viewModel)
        {
            
            var modelIsValid = await _entryService.ValidateModelAsync(viewModel);
            if (!modelIsValid)
            {
                ViewBag.NotificationMessage = "Invalid information. Please re-enter valid information";
                return View(viewModel);
            }

            var customer = await _entryService.GetCustomerAsyncByEmailAsync(viewModel.Email);

            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    DateOfBirth = viewModel.DateOfBirth.Date,
                };

                await _entryService.SubmitCustomerAsync(customer);
            }

            var entry = new Entry
            {
                SerialNumber = viewModel.Serial,
                CustomerId = customer.Id,
                EntryTime = DateTime.Now
            };

            var validAge = await _entryService.ValidateAgeAsync(viewModel);

            if (validAge)
            {
                ViewBag.NotificationMessage = "You must be over 18 to enter into the prize draw.";
                return View(viewModel);
            }

            var existingEntries = await _entryService.ValidateExistingEntries(viewModel.Serial, customer.Id);
  

            if (existingEntries.Item1 < 2)
            {
                await HandleEntry(
                    View(viewModel),
                    entry.SerialNumber,
                    customer.Id,
                    existingEntries.Item2
                    );
            }
            else if (existingEntries.Item1 == 2)
            {
                ViewBag.NotificationMessage = existingEntries.Item2;
                return View(viewModel);
            }

            return View(viewModel);
        }

        private async Task<IActionResult> HandleEntry(ViewResult view, string serialNumber, int customerId, string notificationMessage)
        {
            var entry = new Entry
            {
                SerialNumber = serialNumber,
                CustomerId = customerId,
                EntryTime = DateTime.Now
            };

            await _entryService.SubmitEntryAsync(entry);
            ViewBag.NotificationMessage = notificationMessage;
            return view;
        }


        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 10)
        {
            var viewModel = await _entryService.GetPaginatedEntriesAsync(pageNumber, pageSize);
            return View(viewModel);
        }
    }
}
