using Microsoft.AspNetCore.Mvc;
using Airdnd.Models.DomainModels;            
using Airdnd.Models.DataLayer.Repositories;  

namespace Airdnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ValidationController : Controller
    {
        private IRepository<User> data { get; set; }

        public ValidationController(IRepository<User> rep)
        {
            data = rep;
        }

        public JsonResult CheckOwner(int userId)
        {
            var user = data.Get(userId);

            if (user == null) {
                return Json($"User ID {userId} does not exist.");
            } else if (user.UserType != "Owner") {
                return Json($"User '{user.Name}' is not an 'Owner'.");
            } else {
                TempData["okOwner"] = true; 
                return Json(true);
            }
        }

        public JsonResult CheckEmail(string email)
        {
            var userWithEmail = data.List(new QueryOptions<User> { 
                Where = u => u.Email == email 
            }).FirstOrDefault();

            if (userWithEmail == null) {
                TempData["okEmail"] = true;
                return Json(true);
            } else {
                return Json($"Email '{email}' is already in use.");
            }
        }
    }
}