using Microsoft.AspNetCore.Mvc;
using Airdnd.Models;

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
        private AirdndContext context;

        public ValidationController(AirdndContext ctx)
        {
            context = ctx;
        }
        public JsonResult CheckOwner(int userId)
        {
            var user = context.Users.Find(userId);
            if (user == null) {
                return Json($"User ID {userId} does not exist.");
            } else if (user.UserType != "Owner") {
                return Json($"User '{user.Name}' is not an 'Owner'.");
            } else {
                return Json(true);
            }
        }

        public JsonResult CheckEmail(string email)
        {
            var userWithEmail = context.Users
                .FirstOrDefault(u => u.Email == email);

            if (userWithEmail == null)
            {
                TempData["okEmail"] = true; 
                return Json(true);
            }
            else
            {
                return Json($"Email '{email}' is already in use.");
            }
        }
    }
}