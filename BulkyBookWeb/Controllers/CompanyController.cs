using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.ProductRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: CompanyController
        public async Task<IActionResult> Index()
        {
            IEnumerable<Company> companies = await unitOfWork.CompanyRepository.GetAll();
            return View(companies);
        }

        // GET: CompanyController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var company = await unitOfWork.CompanyRepository.Find(id);

            if(company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CompanyRepository.Add(company);
                var res = await unitOfWork.SaveAsync();
                if (res > 0) TempData["success"] = "create successfully!";
                else TempData["error"] = "Failed to create!";
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: CompanyController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var company = await unitOfWork.CompanyRepository.FirstOrDefault(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CompanyRepository.Update(company);
                var res = await unitOfWork.SaveAsync();
                if (res > 0) TempData["success"] = "Edit successfully!";
                else TempData["error"] = "Failed to edit!";
            }
            return View(company);
        }

        // GET: CompanyController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var company = await unitOfWork.CompanyRepository.FirstOrDefault(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: CompanyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var company = await unitOfWork.CompanyRepository.FirstOrDefault(x => x.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            unitOfWork.CompanyRepository.Remove(company);
            var res = await unitOfWork.SaveAsync();
            if (res > 0) TempData["success"] = "Delete successfully!";
            else TempData["error"] = "Failed to delete!";
            return RedirectToAction("Index");
        }
    }
}
