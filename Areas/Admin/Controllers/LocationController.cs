using Microsoft.AspNetCore.Mvc;
using Airdnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        private AirdndContext context;

        public LocationController(AirdndContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var locations = context.Locations.OrderBy(l => l.Name).ToList();
            return View(locations);
        }

        // --- Add ---
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            return View("Edit", new Location()); 
        }

        [HttpPost]
        public IActionResult Add(Location location)
        {
            if (ModelState.IsValid)
            {
                context.Locations.Add(location);
                context.SaveChanges();
                TempData["message"] = $"{location.Name} was added.";
                return RedirectToAction("Index");
            }
            else
            {
                // Add model-level error
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Add";
                return View("Edit", location); 
            }
        }

        // --- Edit ---
        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewBag.Action = "Edit";
            var location = context.Locations.Find(id);
            if (location == null)
            {
                return RedirectToAction("Index");
            }
            return View(location);
        }

        [HttpPost]
        public IActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                context.Locations.Update(location);
                context.SaveChanges();
                TempData["message"] = $"{location.Name} was updated.";
                return RedirectToAction("Index");
            }
            else
            {
                // Add model-level error
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Edit";
                return View(location);
            }
        }

        // --- Delete ---
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var location = context.Locations.Find(id);
            if (location == null)
            {
                return RedirectToAction("Index");
            }
            return View(location);
        }

        [HttpPost]
        public IActionResult Delete(Location location)
        {
            context.Locations.Remove(location);
            context.SaveChanges();
            TempData["message"] = $"{location.Name} was deleted.";
            return RedirectToAction("Index");
        }
    }
}