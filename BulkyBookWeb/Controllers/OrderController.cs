using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string? name = "")

        {
            IEnumerable<OrderHeader> orders = await unitOfWork.OrderHeaderRepository
                .GetAll(x => !x.OrderStatus.Equals("Deleted") && x.CustomerName.ToUpper().Contains(name.ToUpper().Trim()));
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
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.UtcNow;
            orderHeader.OrderDetails = new List<OrderDetail>();
            var cart = HttpContext.Session.GetString("cart");
            var itemList = JsonConvert.DeserializeObject<List<CartProduct>>(cart);
            foreach (var item in itemList)
            {
                var product = await unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == item.Id);
                var orderDetail = new OrderDetail()
                {
                    OrderHeaderId = orderHeader.Id,
                    ProductId = item.Id,
                    Count = item.Total,
                    Price = product.Price
                };
                orderHeader.OrderDetails.Add(orderDetail);
                orderHeader.OrderTotal += orderDetail.Count * orderDetail.Price;
            }
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
            order.OrderStatus = "Deleted";
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
