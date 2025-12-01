using Microsoft.AspNetCore.Mvc;
using Airdnd.Models.DomainModels;
using Airdnd.Models.DataLayer.Repositories;
using Airdnd.Models.ExtensionMethods;
using Airdnd.Models.Utilities;

namespace Airdnd.Controllers
{
    public class ReservationController : Controller
    {
        private IRepository<Reservation> data { get; set; }
        private IRepository<Residence> residenceData { get; set; }

        public ReservationController(IRepository<Reservation> repo, IRepository<Residence> resRepo)
        {
            data = repo;
            residenceData = resRepo;
        }

        private AirdndSession GetSessionWrapper() => new AirdndSession(HttpContext.Session);
        private AirdndCookies GetCookieWriteWrapper() => new AirdndCookies(HttpContext.Response.Cookies);

        public IActionResult Index()
        {
            var sessionWrapper = GetSessionWrapper();
            var model = sessionWrapper.GetReservations();

            ViewBag.Location = sessionWrapper.GetLocation();
            ViewBag.Dates = sessionWrapper.GetDates();
            ViewBag.Guests = sessionWrapper.GetGuests();

            return View(model);
        }

        [HttpPost]
        public IActionResult Reserve(int residenceId, string selectedDates)
        {
            var sessionWrapper = GetSessionWrapper();
            var cookieWrapper = GetCookieWriteWrapper();
            var reservations = sessionWrapper.GetReservations();

            // Using Repository Get
            var residence = residenceData.Get(residenceId);
            if (residence == null)
            {
                return RedirectToAction("Index", "Residence");
            }

            // Date Parsing via Utility
            var dateRange = DateUtility.ParseDateRange(selectedDates);

            // Server-side validation
            if (!string.IsNullOrEmpty(selectedDates) && selectedDates.ToLower() != "all")
            {
                if (dateRange.StartDate.Date < DateTime.Today)
                {
                    TempData["message"] = "Please select reservation dates from today or later.";
                    return RedirectToAction("Detail", "Residence", new { id = residenceId });
                }
                if (dateRange.EndDate.Date < dateRange.StartDate.Date)
                {
                    TempData["message"] = "End date must be the same as or after the start date.";
                    return RedirectToAction("Detail", "Residence", new { id = residenceId });
                }
            }

            var reservation = new Reservation
            {
                ResidenceId = residenceId,
                ReservationStartDate = dateRange.StartDate,
                ReservationEndDate = dateRange.EndDate
            };

            // Using Repository Insert and Save
            data.Insert(reservation);
            data.Save();

            reservation.Residence = residence; 
            reservations.Add(reservation);

            sessionWrapper.SetReservations(reservations);
            cookieWrapper.SetReservationIds(reservations); 

            TempData["message"] = $"{residence.Name} has been reserved!";

            return RedirectToAction("Index", "Residence", new { 
                SelectedLocation = sessionWrapper.GetLocation(),
                SelectedDates = sessionWrapper.GetDates(),
                SelectedGuests = sessionWrapper.GetGuests()
            }); 
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var sessionWrapper = GetSessionWrapper();
            var cookieWrapper = GetCookieWriteWrapper();

            // Using Repository Get, Delete, Save
            var reservation = data.Get(id);
            if (reservation != null)
            {
                data.Delete(reservation);
                data.Save();
            }

            var reservations = sessionWrapper.GetReservations();
            reservations.RemoveAll(r => r.ReservationId == id);

            sessionWrapper.SetReservations(reservations);
            cookieWrapper.SetReservationIds(reservations); 

            TempData["message"] = "Reservation canceled.";

            return RedirectToAction("Index", "Residence", new { 
                SelectedLocation = sessionWrapper.GetLocation(),
                SelectedDates = sessionWrapper.GetDates(),
                SelectedGuests = sessionWrapper.GetGuests()
            });
        }
    }
}