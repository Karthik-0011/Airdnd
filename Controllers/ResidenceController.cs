using Microsoft.AspNetCore.Mvc;
using Airdnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Airdnd.Controllers
{
    public class ResidenceController : Controller
    {
        private AirdndContext context;

        public ResidenceController(AirdndContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index(HomeViewModel model)
        {
            var session = new AirdndSession(HttpContext!.Session);
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

            IQueryable<Residence> query = context.Residences.Include(r => r.Location).OrderBy(r => r.Name);

            if (model.SelectedLocation != "all")
            {
                query = query.Where(r => r.LocationId == model.SelectedLocation);
            }
            if (model.SelectedGuests > 0)
            {
                query = query.Where(r => r.GuestNumber >= model.SelectedGuests);
            }

            if (model.SelectedDates != "All" && !string.IsNullOrEmpty(model.SelectedDates))
            {
                DateTime reqStart = DateTime.MinValue;
                DateTime reqEnd = DateTime.MaxValue;
                bool parseSuccess = false;

                var dates = model.SelectedDates.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParse(dates[0], out reqStart) &&
                        DateTime.TryParse(dates[1], out reqEnd))
                    {
                        parseSuccess = true;
                    }
                }

                if (parseSuccess)
                {
                    var reservedIds = context.Reservations
                        .Where(rv => rv.ReservationStartDate <= reqEnd && 
                                      rv.ReservationEndDate >= reqStart)
                        .Select(rv => rv.ResidenceId)
                        .Distinct();

                    query = query.Where(r => !reservedIds.Contains(r.ResidenceId));
                }
            }

            model.Residences = query.ToList();
            model.Locations = context.Locations.OrderBy(l => l.Name).ToList();

            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var session = new AirdndSession(HttpContext!.Session);
            var residence = context.Residences.Include(r => r.Location).FirstOrDefault(r => r.ResidenceId == id);
            
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
                    
                    var reservations = context.Reservations
                        .Include(r => r.Residence.Location)
                        .Where(r => intIds.Contains(r.ReservationId))
                        .OrderBy(r => r.ReservationStartDate)
                        .ToList();
                    
                    session.SetReservations(reservations);
                }
            }
        }
    }
}