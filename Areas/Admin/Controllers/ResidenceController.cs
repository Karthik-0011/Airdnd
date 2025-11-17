using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Airdnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResidenceController : Controller
    {
        private AirdndContext context;

        public ResidenceController(AirdndContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var residences = context.Residences
                .Include(r => r.Location)
                .Include(r => r.Owner)
                .OrderBy(r => r.Name)
                .ToList();
                
            return View(residences);
        }

        private void LoadDropdownData()
        {
            ViewBag.Locations = context.Locations
                .OrderBy(l => l.Name)
                .ToList();
            
            ViewBag.Owners = context.Users
                .Where(u => u.UserType == "Owner")
                .OrderBy(u => u.Name)
                .ToList();
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
            if (ModelState.IsValid)
            {
                context.Residences.Add(residence);
                context.SaveChanges();
                TempData["message"] = $"{residence.Name} was added.";
                return RedirectToAction("Index");
            }
            else
            {
                // Add model-level error
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

            var residence = context.Residences.Find(id);
            if (residence == null)
            {
                return RedirectToAction("Index");
            }
            return View(residence);
        }

        [HttpPost]
        public IActionResult Edit(Residence residence)
        {
            if (ModelState.IsValid)
            {
                context.Residences.Update(residence);
                context.SaveChanges();
                TempData["message"] = $"{residence.Name} was updated.";
                return RedirectToAction("Index");
            }
            else
            {
                // Add model-level error
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
            var residence = context.Residences
                .Include(r => r.Location)
                .Include(r => r.Owner)
                .FirstOrDefault(r => r.ResidenceId == id);
            
            if (residence == null)
            {
                return RedirectToAction("Index");
            }
            return View(residence);
        }

        [HttpPost]
        public IActionResult Delete(Residence residence)
        {
            context.Residences.Remove(residence);
            context.SaveChanges();
            TempData["message"] = $"{residence.Name} was deleted.";
            return RedirectToAction("Index");
        }
    }
}