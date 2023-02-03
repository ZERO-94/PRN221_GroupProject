using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BulkyBookWeb.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var cart = HttpContext.Session.GetString("cart");
            
            if(cart == null)
            {
                return View(null);
            }

            var products = JsonConvert.DeserializeObject<List<CartProduct>?>(cart);

            var productIds = products.Select(x => x.Id).ToList();
            var productsInDb = await unitOfWork.ProductRepository.GetAll(x => productIds.Contains(x.Id));

            products.ForEach(x =>
            {
                x.Product = productsInDb.FirstOrDefault(a => a.Id == x.Id);
            });


            return View(products);
        }

        [HttpPost]
        public ActionResult Update(int productId, int amount)
        {
            List<CartProduct> productIds = new List<CartProduct>();
            var cart = HttpContext.Session.GetString("cart");

            if (cart != null) { 
                productIds = JsonConvert.DeserializeObject<List<CartProduct>?>(cart);
            }

            var product = productIds?.FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                if (amount > 0)
                {
                    productIds?.Add(new CartProduct { Id = productId });
                }
                else
                {
                    //TODO: handle remove when null error
                }
            }
            else
            {
                if (amount == 0)
                {
                    productIds?.Remove(product);
                }
                else
                {
                    product.Total = amount;
                }
            }

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(productIds));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(int productId, int amount)
        {
            List<CartProduct>? productIds = null;
            var cart = HttpContext.Session.GetString("cart");

            if(cart == null)
            {
                productIds = new List<CartProduct>();
            } else
            {
                productIds = JsonConvert.DeserializeObject<List<CartProduct>?>(cart);
            }

            var product = productIds?.FirstOrDefault(x => x.Id == productId);
            if(product == null)
            {
                if(amount > 0)
                {
                    productIds.Add(new CartProduct { Id = productId });
                } else
                {
                    //TODO: handle remove when null error
                }
            } else
            {
                if(amount < 0 && product.Total - amount <= 0)
                {
                    productIds.Remove(product);
                } else
                {
                    product.Total += amount;
                }
            }

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(productIds));

            return RedirectToAction("Index", "Product");
        }
    }
}
