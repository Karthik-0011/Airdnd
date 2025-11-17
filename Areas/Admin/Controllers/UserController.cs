using Microsoft.AspNetCore.Mvc;
using Airdnd.Models;
using Microsoft.EntityFrameworkCore;

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private AirdndContext context;

        public UserController(AirdndContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var users = context.Users.OrderBy(u => u.Name).ToList();
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
                if (context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", $"Email '{user.Email}' is already in use.");
                }
            }

            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                context.SaveChanges();
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

            var user = context.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (TempData["okEmail"] == null && !string.IsNullOrEmpty(user.Email))
            {
                if (context.Users.Any(u => u.Email == user.Email && u.UserId != user.UserId))
                {
                    ModelState.AddModelError("Email", $"Email '{user.Email}' is already in use by another user.");
                }
            }

            if (ModelState.IsValid)
            {
                context.Users.Update(user);
                context.SaveChanges();
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
            var user = context.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            var dbUser = context.Users.Find(user.UserId);
            if (dbUser == null)
            {
                return RedirectToAction("Index");
            }
            context.Users.Remove(dbUser);
            context.SaveChanges();
            TempData["message"] = $"{(dbUser.Name ?? user.Name ?? string.Empty)} was deleted.";
            return RedirectToAction("Index");
        }
    }
}