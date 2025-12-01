using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Airdnd.Models.DomainModels;          
using Airdnd.Models.DataLayer.Repositories; 

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResidenceController : Controller
    {
        private IRepository<Residence> data { get; set; }
        private IRepository<Location> locationData { get; set; }
        private IRepository<User> userData { get; set; }

        public ResidenceController(IRepository<Residence> rep, IRepository<Location> locRep, IRepository<User> userRep)
        {
            data = rep;
            locationData = locRep;
            userData = userRep;
        }

        public IActionResult Index()
        {
            var options = new QueryOptions<Residence>
            {
                Includes = "Location, Owner",
                OrderBy = r => r.Name
            };
            var residences = data.List(options);
            return View(residences);
        }

        private void LoadDropdownData()
        {
            ViewBag.Locations = locationData.List(new QueryOptions<Location> { OrderBy = l => l.Name });
            
            ViewBag.Owners = userData.List(new QueryOptions<User> { 
                Where = u => u.UserType == "Owner", 
                OrderBy = u => u.Name 
            });
        }

        // --- Add ---
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadDropdownData(); 
            return View("Edit", new Residence()); 
        }

        [HttpPost]
        public IActionResult Add(Residence residence)
        {
            // Check Owner using Repository
            if (TempData["okOwner"] == null)
            {
                var owner = userData.Get(residence.UserId);
                if (owner == null || owner.UserType != "Owner")
                {
                    ModelState.AddModelError("UserId", "Selected user is not a valid Owner.");
                }
            }

            if (ModelState.IsValid)
            {
                data.Insert(residence);
                data.Save();
                TempData["message"] = $"{residence.Name} was added.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Add";
                LoadDropdownData(); 
                return View("Edit", residence);
            }
        }

        // --- Edit ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadDropdownData(); 
            var residence = data.Get(id);
            if (residence == null) return RedirectToAction("Index");
            return View(residence);
        }

        [HttpPost]
        public IActionResult Edit(Residence residence)
        {
            if (TempData["okOwner"] == null)
            {
                var owner = userData.Get(residence.UserId);
                if (owner == null || owner.UserType != "Owner")
                {
                    ModelState.AddModelError("UserId", "Selected user is not a valid Owner.");
                }
            }

            if (ModelState.IsValid)
            {
                data.Update(residence);
                data.Save();
                TempData["message"] = $"{residence.Name} was updated.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Edit";
                LoadDropdownData(); 
                return View("Edit", residence);
            }
        }

        // --- Delete ---
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var options = new QueryOptions<Residence>
            {
                Where = r => r.ResidenceId == id,
                Includes = "Location, Owner"
            };
            var residence = data.List(options).FirstOrDefault();
            
            if (residence == null) return RedirectToAction("Index");
            return View(residence);
        }

        [HttpPost]
        public IActionResult Delete(Residence residence)
        {
            data.Delete(residence);
            data.Save();
            TempData["message"] = $"{residence.Name} was deleted.";
            return RedirectToAction("Index");
        }
    }
}