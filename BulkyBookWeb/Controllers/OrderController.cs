using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<OrderHeader> orders = new List<OrderHeader>();
            return View(orders);
        }
    }
}
