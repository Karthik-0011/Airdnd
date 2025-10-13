using Microsoft.AspNetCore.Mvc;
namespace Airdnd.Controllers
{
    public class ExperienceController : Controller
    {
        public IActionResult List(string id = "All") => Content($"Experience Controller, List Action, ID: {id}");

        public IActionResult Detail(int id) => Content($"Experience Controller, Detail Action, ID: {id}");
    }
}