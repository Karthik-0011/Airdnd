using Microsoft.AspNetCore.Mvc;
namespace Airdnd.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult List(string id = "All") => Content($"Service Controller, List Action, ID: {id}");

        public IActionResult Detail(int id) => Content($"Service Controller, Detail Action, ID: {id}");
    }
}