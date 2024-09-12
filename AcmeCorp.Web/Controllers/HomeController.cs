using AcmeCorp.Web.Data;
using AcmeCorp.Web.Models;
using AcmeCorp.Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AcmeCorp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!dbContext.SerialNumbers.Any())
            {
                string txtPath = ".\\serial_numbers.txt";

                using (StreamReader reader = new StreamReader(txtPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        var sn = new SerialNumber
                        {
                            Serial = line
                        };

                        dbContext.SerialNumbers.Add(sn);
                    }
                }
                dbContext.SaveChanges();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
