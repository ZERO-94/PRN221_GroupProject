using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await unitOfWork.CategoryRepository.GetAll();
            return View(categories);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category newCategory)
        {
            //custom rule
            if(newCategory.Name == newCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name can't be similar to Display order!");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Add(newCategory);
                var res = await unitOfWork.SaveAsync();
                if (res > 0) TempData["success"] = "create successfully!";
                else TempData["error"] = "Failed to create!";
                return RedirectToAction("Index");
            }
            return View(newCategory);
        }

        //GET
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category editCategory)
        {
            //custom rule
            if (editCategory.Name == editCategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name can't be similar to Display order!");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.CategoryRepository.Update(editCategory);
                var res = await unitOfWork.SaveAsync();
                if (res > 0)
                {
                    TempData["success"] = "Edit successfully!";
                    return RedirectToAction("Index");
                }
                else TempData["error"] = "Failed to edit!";
            }
            return View(editCategory);
        }

        //GET
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var category = await unitOfWork.CategoryRepository.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            unitOfWork.CategoryRepository.Remove(category);
            var res = await unitOfWork.SaveAsync();
            if (res > 0) TempData["success"] = "Delete successfully!";
            else TempData["error"] = "Failed to delete!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            if (String.IsNullOrEmpty(search))
                return RedirectToAction("Index");
            ViewBag.SearchTerm = search;
            IEnumerable<Category> categories = await unitOfWork.CategoryRepository.GetAll(cate => cate.Name.ToUpper().Contains(search.ToUpper().Trim()));
            return View("Index", categories);
        }
    }
}
