using Microsoft.AspNetCore.Mvc;

namespace LevelCounter.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}