using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index(string? name)

        {
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository
                .GetAll(x=>String.IsNullOrWhiteSpace(name)||x.CustomerName.ToUpper().Contains(name.ToUpper().Trim()));
            return View(orders);
        }
        public async Task<IActionResult> GetDetail(int id)
		{
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x=>x.Id == id,
                query => query.Include(x=>x.OrderDetails).ThenInclude(x=>x.Product));
            return View("Detail", order.OrderDetails);
		}
        public async Task<IActionResult> Search(string search)
		{
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");
            ViewBag.SearchTerm = search;
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository.GetAll(x=>x.CustomerName.Contains(search));
            return View("Index", orders);
		}
    }
}
