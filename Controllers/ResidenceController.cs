using Microsoft.AspNetCore.Mvc;
using Airdnd.Models.DomainModels;
using Airdnd.Models.ViewModels;
using Airdnd.Models.DataLayer.Repositories;
using Airdnd.Models.ExtensionMethods;
using Airdnd.Models.Utilities;

namespace Airdnd.Controllers
{
    public class ResidenceController : Controller
    {
        private IRepository<Residence> data { get; set; }
        private IRepository<Location> locationData { get; set; }
        private IRepository<Reservation> reservationData { get; set; }

        public ResidenceController(IRepository<Residence> rep, IRepository<Location> locRep, IRepository<Reservation> resRep)
        {
            data = rep;
            locationData = locRep;
            reservationData = resRep;
        }

        public IActionResult Index(HomeViewModel model)
        {
            var session = new AirdndSession(HttpContext.Session);
            var cookies = new AirdndCookies(Request.Cookies);
            
            CheckCookieReservations(session, cookies);

            bool isRequestFiltered = Request.Method == "POST" ||
                                     Request.Query.ContainsKey(nameof(model.SelectedLocation)) ||
                                     RouteData.Values.ContainsKey(nameof(model.SelectedLocation));

            if (isRequestFiltered)
            {
                session.SetLocation(model.SelectedLocation ?? "all");
                session.SetDates(model.SelectedDates ?? "All");
                session.SetGuests(model.SelectedGuests);
            }
            else
            {
                model.SelectedLocation = session.GetLocation();
                model.SelectedDates = session.GetDates();
                model.SelectedGuests = session.GetGuests();
            }

            // --- REPOSITORY QUERY LOGIC ---
            var options = new QueryOptions<Residence>
            {
                Includes = "Location",
                OrderBy = r => r.Name
            };

            if (model.SelectedLocation != "all")
            {
                options.Where = r => r.LocationId == model.SelectedLocation;
            }

            var residences = data.List(options);

            // Memory Filtering for Guests
            if (model.SelectedGuests > 0)
            {
                residences = residences.Where(r => r.GuestNumber >= model.SelectedGuests);
            }

            // Memory/Repo Filtering for Dates
            if (model.SelectedDates != "All" && !string.IsNullOrEmpty(model.SelectedDates))
            {
                var dateRange = DateUtility.ParseDateRange(model.SelectedDates);
                
                var resOptions = new QueryOptions<Reservation>
                {
                    Where = rv => rv.ReservationStartDate <= dateRange.EndDate && 
                                  rv.ReservationEndDate >= dateRange.StartDate
                };
                
                var reservedIds = reservationData.List(resOptions)
                    .Select(rv => rv.ResidenceId)
                    .Distinct();

                residences = residences.Where(r => !reservedIds.Contains(r.ResidenceId));
            }

            model.Residences = residences.ToList();
            model.Locations = locationData.List(new QueryOptions<Location> { OrderBy = l => l.Name }).ToList();

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var session = new AirdndSession(HttpContext.Session);
            
            var options = new QueryOptions<Residence> 
            { 
                Where = r => r.ResidenceId == id,
                Includes = "Location" 
            };

            var residence = data.List(options).FirstOrDefault();
            
            if (residence == null) { return RedirectToAction("Index"); }

            ViewBag.Location = session.GetLocation();
            ViewBag.Dates = session.GetDates();
            ViewBag.Guests = session.GetGuests();

            return View(residence);
        }

        private void CheckCookieReservations(AirdndSession session, AirdndCookies cookies)
        {
            if (session.GetReservationCount() == 0)
            {
                string[] ids = cookies.GetReservationIds();
                if (ids.Length > 0)
                {
                    var intIds = new List<int>();
                    foreach (var id in ids) { if (int.TryParse(id, out int intId)) { intIds.Add(intId); } }
                    
                    var options = new QueryOptions<Reservation>
                    {
                        Where = r => intIds.Contains(r.ReservationId),
                        Includes = "Residence.Location",
                        OrderBy = r => r.ReservationStartDate
                    };

                    var reservations = reservationData.List(options).ToList();
                    session.SetReservations(reservations);
                }
            }
        }
    }
}