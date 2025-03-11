using Microsoft.AspNetCore.Mvc;
using WebApplication123.Infrastructure.IRepository;
using WebApplication123.Models;

namespace WebApplication123.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int pg = 1)
        {
            // var categories = _unitOfWork.Category.GetAll();
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();

            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int recsCount = categories.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = categories.Skip(recSkip).Take(pageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
            //return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category, int id)
        {
            var existingCategory = _unitOfWork.Category.GetT(x => x.Name == category.Name && x.Id != id);
            if (existingCategory != null)
            {

                ModelState.AddModelError("Name", "A category with the same name already exists, Please Create another Category.");
                return View(category);
            }
            category.Id = id;
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Done!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {

            var existingCategory = _unitOfWork.Category.GetT(x => x.Name == category.Name && x.Id != id);
            if (existingCategory != null)
            {

                ModelState.AddModelError("Name", "A category with the same name already exists Please Update another Category.");
                return View(category);
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Update Done!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _unitOfWork.Category.GetT(x => x.Id == id);
            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
