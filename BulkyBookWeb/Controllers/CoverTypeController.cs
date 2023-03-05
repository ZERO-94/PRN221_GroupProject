using BulkyBook.BusinessObject.Models;
using BulkyBook.BusinessObject.Utilities;
using BulkyBook.BusinessObject.ViewModels;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Authorize(Roles = Role.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;       
        }

        // GET: CoverTypeController
        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            int pageSize = 8;
            var result = await unitOfWork.CoverTypeRepository.Pagination(page, pageSize, x => !x.Name.Contains("Deleted") && x.Name.Contains(search));
            ViewBag.SearchTerm = search;
            return View(new PaginationViewModel<CoverType>()
            {
                Total = result.Item1,
                Data = result.Item2,
                TotalPage = (int?)((result.Item1 + pageSize - 1) / pageSize) ?? 0,
            });
        }

        // GET: CoverTypeController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: CoverTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoverType coverType)
        {
            if(ModelState.IsValid)
            {
                unitOfWork.CoverTypeRepository.Add(coverType);
                var res = await unitOfWork.SaveAsync();
                if(res > 0)
                {
                    TempData["success"] = "Create Successfully!";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Failed to create!";
            }
            return View(coverType);
        }

        // GET: CoverTypeController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var coverType = await unitOfWork.CoverTypeRepository.FirstOrDefault(x => x.Id == id);
            if(coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CoverTypeRepository.Update(coverType);
                var res = await unitOfWork.SaveAsync();
                if (res > 0)
                {
                    TempData["success"] = "Edit Successfully!";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "Failed to edit!";
            }
            return View(coverType);
        }

        // GET: CoverTypeController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var coverType = await unitOfWork.CoverTypeRepository.FirstOrDefault(x => x.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        // POST: CoverTypeController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var coverType = await unitOfWork.CoverTypeRepository.FirstOrDefault(x => x.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            unitOfWork.CoverTypeRepository.Remove(coverType);
            var res = await unitOfWork.SaveAsync();
            if (res > 0)
            {
                TempData["success"] = "Edit Successfully!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Failed to edit!";
            return RedirectToAction("Index");
        }
    }
}
