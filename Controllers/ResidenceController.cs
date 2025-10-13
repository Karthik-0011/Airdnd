using Microsoft.AspNetCore.Mvc;
namespace Airdnd.Controllers
{
    public class ResidenceController : Controller
    {
        public IActionResult List(string id = "All") => Content($"Residence Controller, List Action, ID: {id}");

        public IActionResult Detail(int id) => Content($"Residence Controller, Detail Action, ID: {id}");
    }
}