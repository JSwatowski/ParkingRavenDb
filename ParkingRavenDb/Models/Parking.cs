namespace ParkingRavenDb.Models
{
    public class Parking
    {
        public string Id { get; set; }
        public string? ParkingAreaName { get; set; }
        public int WeekdaysHourlyRate { get; set; }
        public int WeekendHourlyRate { get; set; }
        public int DicountPercentage { get; set; }

    }
}
