using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;       
        }

        // GET: CoverTypeController
        public async Task<IActionResult> Index()
        {
            IEnumerable<CoverType> coverTypes = await unitOfWork.CoverTypeRepository.GetAll();
            return View(coverTypes);
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
