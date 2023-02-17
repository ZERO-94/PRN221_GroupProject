using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
