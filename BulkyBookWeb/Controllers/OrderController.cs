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

        public async Task<IActionResult> Index(string? name="")

        {
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository
                .GetAll(x => !x.CustomerName.Contains("$(Deleted)") && x.CustomerName.ToUpper().Contains(name.ToUpper().Trim()));
            return View(orders);
        }
        public async Task<IActionResult> GetDetail(int id)
        {
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            return View("Detail", order.OrderDetails);
        }
        public async Task<IActionResult> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");
            ViewBag.SearchTerm = search;
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository.GetAll(x => x.CustomerName.Contains(search));
            return View("Index", orders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.UtcNow;
            orderHeader.ShippingDate = orderHeader.OrderDate.AddDays(2);
            orderHeader.TrackingNumber = "123";
            //order detail not yet added
            var orderDetails = await unitOfWork.OrderDetailRepository.GetAll(x => x.OrderHeaderId == 1);
            double total = 0;
            if (orderDetails == null)
            {
                TempData["error"] = "Order have no item";
                return View();
            }
            else
			{
				foreach (var orderDetail in orderDetails)
				{
                    total += orderDetail.Count * orderDetail.Price;
				}
			}
            orderHeader.OrderTotal = total;
            unitOfWork.OrderHeaderRepository.Add(orderHeader);
            var res = await unitOfWork.SaveAsync();
            if (res > 0) TempData["success"] = "create successfully!";
            else TempData["error"] = "Failed to create!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
		{
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            return View(order);
		}
        [HttpPost]
        public async Task<IActionResult> Edit(OrderHeader order)
		{
            unitOfWork.OrderHeaderRepository.Update(order);
            var res = await unitOfWork.SaveAsync();
            if (res > 0)
            {
                TempData["success"] = "Edit successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Failed to edit!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            order.CustomerName += " $(Deleted)";
            unitOfWork.OrderHeaderRepository.Update(order);
            var res = await unitOfWork.SaveAsync();
            if (res > 0)
            {
                TempData["success"] = "Delete successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Failed to delete!";
            return View();
        }
    }
}
