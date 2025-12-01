using Airdnd.Models.DomainModels;

namespace Airdnd.Models.Utilities
{
    public class AirdndCookies
    {
        private const string ReservationsKey = "reservations";
        private const string Delimiter = "-";
        private const int CookieExpiryDays = 60; // 2 Months

        private IRequestCookieCollection? requestCookies { get; set; }
        private IResponseCookies? responseCookies { get; set; }
        public AirdndCookies(IRequestCookieCollection cookies)
        {
            requestCookies = cookies;
        }
        public AirdndCookies(IResponseCookies cookies)
        {
            responseCookies = cookies;
        }

        public void SetReservationIds(List<Reservation> reservations)
        {
            List<string> ids = reservations.Select(r => r.ReservationId.ToString()).ToList();
            string idsString = string.Join(Delimiter, ids);

            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(CookieExpiryDays)
            };

            RemoveReservationIds();
            responseCookies?.Append(ReservationsKey, idsString, options);
        }

        public string[] GetReservationIds()
        {
            if (requestCookies == null) return Array.Empty<string>();
            string cookie = requestCookies[ReservationsKey] ?? string.Empty;
            if (string.IsNullOrEmpty(cookie))
                return Array.Empty<string>();
            else
                return cookie.Split(Delimiter);
        }

        public void RemoveReservationIds()
        {
            responseCookies?.Delete(ReservationsKey);
        }
    }
}