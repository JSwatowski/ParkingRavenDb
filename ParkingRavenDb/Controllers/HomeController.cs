using Microsoft.AspNetCore.Mvc;
using ParkingRavenDb.Models;
using Raven.Client.Documents;
using System.Diagnostics;

namespace ParkingRavenDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())

            using (var session = documentstore.OpenSession())
            {
                /*
                session.Store(new Parking
                {
                    ParkingAreaName = "Parking Name 1",
                    WeekdaysHourlyRate = 2.5m,
                    WeekendHourlyRate = 5.2m,
                    DicountPercentage = 10

                });

                session.SaveChanges();
                
                var allParking = session.Query<Parking>().ToArray();
                */
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