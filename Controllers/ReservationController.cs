using Microsoft.AspNetCore.Mvc;
using Airdnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Airdnd.Controllers
{
    public class ReservationController : Controller
    {
        private AirdndContext context;

        public ReservationController(AirdndContext ctx)
        {
            context = ctx;
        }

        private AirdndSession GetSessionWrapper()
        {
            return new AirdndSession(HttpContext!.Session);
        }

        private AirdndCookies GetCookieWriteWrapper()
        {
            return new AirdndCookies(HttpContext!.Response.Cookies);
        }

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

            var residence = context.Residences.Find(residenceId);
            if (residence == null)
            {
                return RedirectToAction("Index", "Residence");
            }

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

            context.Reservations.Add(reservation);
            context.SaveChanges(); 

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

            var reservation = context.Reservations.Find(id);
            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                context.SaveChanges();
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