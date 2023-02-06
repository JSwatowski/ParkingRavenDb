using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingRavenDb.Models;
using Raven.Client.Documents;

namespace ParkingRavenDb.Controllers
{
    public class ParkingController : Controller
    {
        public IActionResult Index()
        {
            List<Parking> parking = new List<Parking>();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())

            using (var session = documentstore.OpenSession())
            {

                parking = session.Query<Parking>().ToList();
            }
            return View(parking);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Parking parking)
        {
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())
            using (var session = documentstore.OpenSession())
            {
                if (ModelState.IsValid)
                {
                    session.Store(parking);
                    session.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(parking);
        }



        [HttpGet]
        public IActionResult Payment()
        {
            PaymentVM vm = new PaymentVM();
            List<Parking> parking = new List<Parking>();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())

            using (var session = documentstore.OpenSession())
            {
                parking = session.Query<Parking>().ToList();
            }
            vm.ParkingList = new SelectList(parking,"Id", "ParkingAreaName");
            return View(vm);
        }

        [HttpPost]
        public IActionResult Payment(PaymentVM vm)
        {
            Parking parking = new Parking();
            double payment = 0;

            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())

            using (var session = documentstore.OpenSession())
            {
                parking = session.Load<Parking>(vm.Selected.ToString());
            }
            if ((vm.ParkingDate.DayOfWeek == DayOfWeek.Saturday) || (vm.ParkingDate.DayOfWeek == DayOfWeek.Sunday))
            {
                System.TimeSpan time = vm.TimeTo - vm.TimeFrom;
                if (parking.DicountPercentage != 0)
                {
                    payment = ((double)time.TotalHours * parking.WeekendHourlyRate * (100 - parking.DicountPercentage)) / 100;
                }else
                {
                    payment = (double)time.TotalHours * parking.WeekendHourlyRate;

                }
            }
            else
            {
                System.TimeSpan time = vm.TimeTo - vm.TimeFrom;
                if (parking.DicountPercentage != 0)
                {
                    payment = ((double)time.TotalHours * parking.WeekdaysHourlyRate * (100 - parking.DicountPercentage)) / 100;
                }
                else
                {
                    payment = (double)time.TotalHours * parking.WeekdaysHourlyRate;
                    payment = Math.Round(payment, 2);
                }
            }
            ViewData["amountForPay"] = payment;

            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            Parking parking = new Parking();

            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())
            using (var session = documentstore.OpenSession())
            {
                parking = session.Load<Parking>(Id);
            }

            return View(parking);
        }
        [HttpPost]
        public IActionResult Edit(Parking parking)
        {

            Parking parkingEdit = new Parking();

            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())
            using (var session = documentstore.OpenSession())
            {
                parkingEdit = session.Load<Parking>(parking.Id);

                if(parkingEdit != null)
                {
                    parkingEdit.Id = parking.Id;
                    parkingEdit.ParkingAreaName = parking.ParkingAreaName;
                    parkingEdit.WeekendHourlyRate = parking.WeekendHourlyRate; 
                    parkingEdit.WeekdaysHourlyRate = parking.WeekdaysHourlyRate;    
                    parkingEdit.DicountPercentage = parking.DicountPercentage;
                    session.SaveChanges();
                }
                return RedirectToAction("Index");
            }   
        }

        public IActionResult Delete(string Id)
        {

            Parking parking = new Parking();

            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080/" },
                Database = "Parking"
            }.Initialize())
            using (var session = documentstore.OpenSession())
            {
                parking = session.Load<Parking>(Id);

                session.Delete<Parking>(parking);
                session.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
