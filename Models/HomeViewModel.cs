namespace Airdnd.Models
{
    public class HomeViewModel
    {
        public List<Residence> Residences { get; set; } = new List<Residence>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public string SelectedLocation { get; set; } = "All";
        public string SelectedDates { get; set; } = "All";
        public int SelectedGuests { get; set; } = 1;
        public string CheckActiveLocation(string locationId) => locationId == SelectedLocation ? "active" : "";
    } 
}