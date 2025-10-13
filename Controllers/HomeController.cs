using Microsoft.AspNetCore.Mvc;
namespace Airdnd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Support() => Content("Home Controller, Support Action");
        public IActionResult Cancellation() => Content("Home Controller, Cancellation Action");
        public IActionResult Terms() => Content("Home Controller, Terms Action");
        public IActionResult Cookie() => Content("Home Controller, Cookie Action");
    }
}