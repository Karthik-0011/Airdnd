namespace Airdnd.Models 
{ public class Residence
    {
        public int ResidenceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ResidencePicture { get; set; } = string.Empty;
        public int GuestNumber { get; set; }
        public int BedroomNumber { get; set; }
        public int BathroomNumber { get; set; }
        public double PricePerNight { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public Location Location { get; set; } = null!;
    } 
}