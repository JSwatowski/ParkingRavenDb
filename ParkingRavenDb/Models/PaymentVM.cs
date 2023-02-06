using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkingRavenDb.Models
{
    public class PaymentVM
    {

        public SelectList ParkingList { get; set; }
        public DateTime ParkingDate { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int Selected { get; set; } 
    }
}
