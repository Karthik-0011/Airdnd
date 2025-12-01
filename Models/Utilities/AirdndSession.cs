using Airdnd.Models.ExtensionMethods;
using Airdnd.Models.DomainModels;

namespace Airdnd.Models.Utilities
{
    public class AirdndSession
    {
        private const string ReservationsKey = "reservations";
        private const string LocationKey = "location";
        private const string DatesKey = "dates";
        private const string GuestsKey = "guests";

        private ISession session { get; set; }

        public AirdndSession(ISession session)
        {
            this.session = session;
        }
        public void SetReservations(List<Reservation> reservations) => session.SetObject(ReservationsKey, reservations);
        public List<Reservation> GetReservations() => session.GetObject<List<Reservation>>(ReservationsKey) ?? new List<Reservation>();
        public void RemoveReservations() => session.Remove(ReservationsKey);
        public int GetReservationCount() => GetReservations().Count;

        public void SetLocation(string location) => session.SetString(LocationKey, location ?? "all");
        public string GetLocation() => session.GetString(LocationKey) ?? "all";
        
        public void SetDates(string dates) => session.SetString(DatesKey, dates ?? "All");
        public string GetDates() => session.GetString(DatesKey) ?? "All";

        public void SetGuests(int guests) => session.SetInt32(GuestsKey, guests);
        public int GetGuests() => session.GetInt32(GuestsKey) ?? 1;
    }
}