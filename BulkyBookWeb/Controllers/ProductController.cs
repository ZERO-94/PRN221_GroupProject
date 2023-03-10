using BulkyBook.BusinessObject.Models;
using BulkyBook.BusinessObject.Utilities;
using BulkyBook.BusinessObject.ViewModels;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.IO;

namespace BulkyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string search="", int page=1)
        {
            int pageSize = 8;
            ViewBag.SearchTerm = search;
            var result = await unitOfWork.ProductRepository.Pagination(
                    page, pageSize ,
                    x=> x.Status != "Deleted" && (String.IsNullOrWhiteSpace(search) || x.Title.Contains(search.Trim())),
                    (query) => query.Include(x => x.Category).Include(x => x.CoverType)
                );
            TempData["searchValue"] = String.IsNullOrWhiteSpace(search) ? "" : search;

            return View(new PaginationViewModel<Product>()
            {
                Total = result.Item1,
                Data= result.Item2,
                TotalPage = (int?)((result.Item1 + pageSize - 1) / pageSize) ?? 0,
            });
        }

        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await unitOfWork.ProductRepository
                .FirstOrDefault(x => x.Id == id,
                query => query.Include(x => x.Category).Include(x => x.CoverType));

            if (product == null)
            {
                return NotFound();
            }
            product.ImageUrl = Request.Scheme + "://" + Path.Combine(Request.Host.Value, product.ImageUrl).Replace("\\", "/");
            return View("Detail", product);
        }

        //GET
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                CategoryList = (await unitOfWork.CategoryRepository.GetAll()).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }),
                CoverTypeList = (await unitOfWork.CoverTypeRepository.GetAll()).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
            };

            if (id == null || id == 0)
            {
                //create
                productViewModel.Product = new Product();
            }
            else
            {
                //update
                var productFromDb = await unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == id);
                if (productFromDb == null) return NotFound();

                productViewModel.Product = productFromDb;
            }

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            var product = productViewModel.Product;

            if (product.Id == 0)
            {
                //create
                if (ModelState.IsValid)
                {
                    //add file
                    if (file != null)
                    {
                        var wwwRootPath = webHostEnvironment.WebRootPath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(file.FileName);
                        var storedPath = Path.Combine(wwwRootPath, "images", "products", fileName + extension);

                        using (var fileStream = new FileStream(storedPath, mode: FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        product.ImageUrl = Path.Combine("images", "products", fileName + extension);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Uploaded file is empty or null");
                        TempData["error"] = "Uploaded file is empty or null!";
                        return View(productViewModel);
                    }

                    unitOfWork.ProductRepository.Add(product);
                    var res = await unitOfWork.SaveAsync();
                    if (res > 0)
                    {
                        TempData["success"] = "Create successfully!";
                        return RedirectToAction("Index");
                    }
                    else TempData["error"] = "Failed to create!";
                }
            }
            else
            {
                //update
                if (ModelState.IsValid)
                {
                    //add file
                    if (file != null)
                    {
                        var wwwRootPath = webHostEnvironment.WebRootPath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(file.FileName);
                        var storedPath = Path.Combine(wwwRootPath, "images", "products", fileName + extension);

                        using (var fileStream = new FileStream(storedPath, mode: FileMode.Create))
                        {
                            //delete before add
                            if (System.IO.File.Exists(Path.Combine(wwwRootPath, product.ImageUrl)))
                            {
                                System.IO.File.Delete(Path.Combine(wwwRootPath, product.ImageUrl));
                            }
                            file.CopyTo(fileStream);
                        }
                        product.ImageUrl = Path.Combine("images", "products", fileName + extension);
                    }

                    unitOfWork.ProductRepository.Update(product);
                    var res = await unitOfWork.SaveAsync();
                    TempData["success"] = "Edit successfully!";
                }
            }

            return RedirectToAction("Index");
        }

        //GET
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.ImageUrl = Request.Scheme + "://" + Path.Combine(Request.Host.Value, product.ImageUrl).Replace("\\", "/");

            return View(product);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Role_Admin)]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var product = await unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

			product.Status = "Deleted";
			unitOfWork.ProductRepository.Update(product);
			var res = await unitOfWork.SaveAsync();
			if (res > 0) TempData["success"] = "Delete successfully!";
            else TempData["error"] = "Failed to delete!";
            return RedirectToAction("Index");
        }
    }
}
