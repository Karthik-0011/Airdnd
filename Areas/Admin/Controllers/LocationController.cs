using Microsoft.AspNetCore.Mvc;
using Airdnd.Models.DomainModels;            
using Airdnd.Models.DataLayer.Repositories;  

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        // Using IRepository instead of Context
        private IRepository<Location> data { get; set; }

        public LocationController(IRepository<Location> rep)
        {
            data = rep;
        }

        public IActionResult Index()
        {
            // Using QueryOptions for sorting
            var options = new QueryOptions<Location> { OrderBy = l => l.Name };
            var locations = data.List(options);
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
            if (!string.IsNullOrEmpty(location.LocationId))
            {
                location.LocationId = location.LocationId.ToLower();
            }

            // Using Repository Get()
            var existing = data.Get(location.LocationId);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(Location.LocationId), "This Location ID already exists.");
            }

            if (ModelState.IsValid)
            {
                // Using Repository Insert and Save
                data.Insert(location);
                data.Save();
                TempData["message"] = $"{location.Name} was added.";
                return RedirectToAction("Index");
            }
            else
            {
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
            var location = data.Get(id); // Using Repository Get
            if (location == null) return RedirectToAction("Index");
            return View(location);
        }

        [HttpPost]
        public IActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                data.Update(location); // Using Repository Update
                data.Save();
                TempData["message"] = $"{location.Name} was updated.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Edit";
                return View(location);
            }
        }

        // --- Delete ---
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var location = data.Get(id);
            if (location == null) return RedirectToAction("Index");
            return View(location);
        }

        [HttpPost]
        public IActionResult Delete(Location location)
        {
            data.Delete(location); // Using Repository Delete
            data.Save();
            TempData["message"] = $"{location.Name} was deleted.";
            return RedirectToAction("Index");
        }
    }
}