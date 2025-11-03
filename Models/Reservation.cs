namespace Airdnd.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        public int ResidenceId { get; set; }
        public Residence Residence { get; set; } = null!;
    } 
}