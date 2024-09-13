using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AcmeCorp.Data.Db;
using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Models;
using AcmeCorp.Service.Services;

namespace AcmeCorp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISerialService _serialService;
        //private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ISerialService serialService)
        {
            _logger = logger;
            _serialService = serialService;
            //this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _serialService.OccupySerialNumberDatabase();
            //if (!dbContext.SerialNumbers.Any())
            //{
            //    string txtPath = ".\\serial_numbers.txt";

            //    using (StreamReader reader = new StreamReader(txtPath))
            //    {
            //        string line;
            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            line = line.Trim();
            //            var sn = new SerialNumber
            //            {
            //                Serial = line
            //            };

            //            dbContext.SerialNumbers.Add(sn);
            //        }
            //    }
            //    dbContext.SaveChanges();
            //}

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
