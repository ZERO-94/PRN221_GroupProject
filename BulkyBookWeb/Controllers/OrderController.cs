using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()

        {
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository.GetAll();
            return View(orders);
        }
        public async Task<IActionResult> GetDetail(int id)
		{
            var orderDetails = await unitOfWork.OrderDetailRepository.GetAll();
            return View("Detail",orderDetails);
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
