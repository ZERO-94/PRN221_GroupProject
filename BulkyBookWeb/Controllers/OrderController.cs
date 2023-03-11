using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using BulkyBook.BusinessObject.Utilities;
using BulkyBook.BusinessObject.ViewModels;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BulkyBookWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Authorize(Roles = Role.Role_Admin + ", " + Role.Role_User_Customer)]
        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            ViewBag.SearchTerm = search;
            int pageSize = 8;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return View();
            var result = await unitOfWork.OrderHeaderRepository
                .Pagination(page, pageSize, x => User.IsInRole("Admin")
                            ? !x.OrderStatus.Equals("Deleted") && x.CustomerName.ToUpper().Contains(search.ToUpper().Trim())
                            : !x.OrderStatus.Equals("Deleted") && x.ApplicationUserId.Equals(user.Id));

            return View(new PaginationViewModel<OrderHeader>()
            {
                Total = result.Item1,
                Data = result.Item2,
                TotalPage = (int?)((result.Item1 + pageSize - 1) / pageSize) ?? 0,
            });
        }
        [Authorize(Roles = Role.Role_Admin + ", " + Role.Role_User_Customer)]
        public async Task<IActionResult> GetDetail(int id)
        {
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            return View("Detail", order);
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
        [Authorize(Roles = Role.Role_User_Customer)]
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated == false)
			{
                TempData["error"] = "You must login before checking out";
                return RedirectToAction("Index", "ShoppingCart");
			}
            return View();
        }
        [HttpPost]
        [Authorize(Roles = Role.Role_User_Customer)]
        public async Task<IActionResult> Create(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.Now;
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
                    Price = product.Price,
                    ProductTitle= product.Title,
                    ProductAuthor= product.Author,
                    ProductDescription= product.Description,
                    ProductISBN= product.ISBN,
                };
                orderHeader.OrderDetails.Add(orderDetail);
                orderHeader.OrderTotal += orderDetail.Count * orderDetail.Price;
            }
            unitOfWork.OrderHeaderRepository.Add(orderHeader);
            var res = await unitOfWork.SaveAsync();
            if (res > 0)
            {
                TempData["success"] = "create successfully!";
                //clear cart 
                HttpContext.Session.Remove("cart");
            }
            else TempData["error"] = "Failed to create!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));
            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> Edit(OrderHeader order)
        {
            if(order.ShippingDate <= DateTime.UtcNow) {
				TempData["error"] = "Shipping date should be after today!";
                return View(order);
			}
            var orderDb = await unitOfWork.OrderHeaderRepository
                .FirstOrDefault(x => x.Id == order.Id,
                query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));

            if (orderDb.OrderStatus == "Canceled" || orderDb.OrderStatus == "Completed")
            {
                TempData["error"] = "Invalid order status to edit!";
                return View(order);
            }

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

        [HttpPost]
		[Authorize(Roles = Role.Role_User_Customer)]
		public async Task<IActionResult> CancelOrder(int id)
        {
			var order = await unitOfWork.OrderHeaderRepository
				.FirstOrDefault(x => x.Id == id,
				query => query.Include(x => x.OrderDetails).ThenInclude(x => x.Product));

            if(order.OrderStatus != "Ordered")
            {
				TempData["error"] = "Invalid order status to cancel!";
				return RedirectToAction("GetDetail", new { id = id });
			}

            order.OrderStatus = "Canceled";
			unitOfWork.OrderHeaderRepository.Update(order);
			var res = await unitOfWork.SaveAsync();
			if (res > 0)
			{
				TempData["success"] = "Cancel successfully!";
			}
			else TempData["error"] = "Failed to cancel order!";
			return RedirectToAction("GetDetail", new {id = id});
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
