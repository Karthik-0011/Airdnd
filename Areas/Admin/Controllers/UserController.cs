using Microsoft.AspNetCore.Mvc;
using Airdnd.Models.DomainModels;           
using Airdnd.Models.DataLayer.Repositories;  

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private IRepository<User> data { get; set; }

        public UserController(IRepository<User> rep)
        {
            data = rep;
        }

        public IActionResult Index()
        {
            var users = data.List(new QueryOptions<User> { OrderBy = u => u.Name });
            return View(users);
        }

        private void LoadViewBagData()
        {
            ViewBag.UserTypes = new List<string> { "Owner", "Admin", "Client" };
        }

        // --- Add ---
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadViewBagData();
            return View("Edit", new User()); 
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            if (TempData["okEmail"] == null && !string.IsNullOrEmpty(user.Email))
            {
                // Check email uniqueness using Repository
                var existingUser = data.List(new QueryOptions<User> { 
                    Where = u => u.Email == user.Email 
                }).FirstOrDefault();

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", $"Email '{user.Email}' is already in use.");
                }
            }

            if (ModelState.IsValid)
            {
                data.Insert(user);
                data.Save();
                TempData["message"] = $"{user.Name} was added.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Add";
                LoadViewBagData();
                return View("Edit", user);
            }
        }

        // --- Edit ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            LoadViewBagData();
            var user = data.Get(id);
            if (user == null) return RedirectToAction("Index");
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (TempData["okEmail"] == null && !string.IsNullOrEmpty(user.Email))
            {
                var existingUser = data.List(new QueryOptions<User> { 
                    Where = u => u.Email == user.Email && u.UserId != user.UserId 
                }).FirstOrDefault();

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", $"Email '{user.Email}' is already in use by another user.");
                }
            }

            if (ModelState.IsValid)
            {
                data.Update(user);
                data.Save();
                TempData["message"] = $"{user.Name} was updated.";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Please fix the error(s) below.");
                ViewBag.Action = "Edit";
                LoadViewBagData();
                return View(user);
            }
        }

        // --- Delete ---
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = data.Get(id);
            if (user == null) return RedirectToAction("Index");
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            var dbUser = data.Get(user.UserId);
            if (dbUser == null) return RedirectToAction("Index");
            
            data.Delete(dbUser);
            data.Save();
            TempData["message"] = $"{(dbUser.Name ?? user.Name ?? string.Empty)} was deleted.";
            return RedirectToAction("Index");
        }
    }
}