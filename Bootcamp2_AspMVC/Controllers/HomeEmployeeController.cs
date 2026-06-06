using Microsoft.AspNetCore.Mvc;

namespace Bootcamp2_AspMVC.Controllers
{
    public class HomeEmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
