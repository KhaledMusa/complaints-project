using Microsoft.AspNetCore.Mvc;

namespace Complains_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
