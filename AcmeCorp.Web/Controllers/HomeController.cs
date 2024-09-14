using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AcmeCorp.Data.Models;
using AcmeCorp.Service.Services;

namespace AcmeCorp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISerialService _serialService;

        public HomeController(ILogger<HomeController> logger, ISerialService serialService)
        {
            _logger = logger;
            _serialService = serialService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //_serialService.OccupySerialNumberDatabase();
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
