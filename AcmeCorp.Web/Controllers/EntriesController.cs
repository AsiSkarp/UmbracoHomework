using AcmeCorp.Web.Data;
using AcmeCorp.Web.Models;
using AcmeCorp.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorp.Web.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EntriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEntryViewModel viewModel)
        {
            var customer = await dbContext.Customers
                .FirstOrDefaultAsync(
                c => c.Email == viewModel.Email);

            if (customer == null)
            {
                customer = new Customer
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    DateOfBirth = viewModel.DateOfBirth.Date,
                };

                await dbContext.Customers.AddAsync(customer);
                await dbContext.SaveChangesAsync();
            }

            var age = DateTime.Now.Year - viewModel.DateOfBirth.Year;
            if (viewModel.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                ViewBag.NotificationMessage = "You must be over 18 to enter into the prize draw.";
                return View(viewModel);
            }

            var serialNumber = await dbContext.SerialNumbers
                .FirstOrDefaultAsync(s => s.Serial == viewModel.Serial);

            if (serialNumber == null)
            {
                ViewBag.NotificationMessage = "Invalid serial number. Please enter valid serial number.";
                return View(viewModel);
            }

            var existingEntries= await dbContext.Entries
                .Where(e => e.SerialNumber == serialNumber.Serial)
                .ToListAsync();

            if (existingEntries.Count == 0)
            {
                await HandleEntry(
                    View(viewModel),
                    serialNumber.Serial,
                    customer.Id,
                    "You have successfully entered once into the prize draw. You may enter the prize draw once more for this serial number"
                    );
            }
            else if (existingEntries.Count == 1 && existingEntries[0].CustomerId == customer.Id)
            {
                await HandleEntry(
                    View(viewModel),
                    serialNumber.Serial,
                    customer.Id,
                    "You have successfully entered twice for the prize draw. You may enter into the prize draw for any other valid serial number(s)"
                    );
            }
            else if (existingEntries.Count == 2 || (existingEntries.Count == 1 && existingEntries[0].CustomerId != customer.Id )) 
            {
                ViewBag.NotificationMessage = "This serial number already been entered twice, or is registered to another user. Please input another serial number.";
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

            await dbContext.Entries.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            ViewBag.NotificationMessage = notificationMessage;
            return view;
        }

        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 10)
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

            var pagedResults = await query
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            var viewModel = new PaginatedListViewModel
            {
                Entries = pagedResults,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

            return View(viewModel);
            //var results = await query.ToListAsync();
            
            //return View(results);
        }
    }
}
