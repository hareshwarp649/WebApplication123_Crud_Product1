using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication123.Data;
using WebApplication123.Models;

namespace WebApplication123.Controllers
{
    public class ProductController : Controller
    {
        private readonly WebAppDbContext _context;

        public ProductController(WebAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include("Categories");
            return View(products);
            //IEnumerable<Product> products = _context.Products;
            //return View(products);

        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();

        }
        [NonAction]
        private void LoadCategories()
        {
            LoadCategories();
            var catego=_context.Categories.ToList();
            ViewBag.Categories = new SelectList(catego, "Id", "Name");
        }
        [HttpPost]
        public IActionResult Create(Product model)
        {
            model.Id = 0;
            if (ModelState.IsValid)
            {
                _context.Products.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();


        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                NotFound();
            }
            LoadCategories();
            var product = _context.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            ModelState.Remove("Categories");
            if (ModelState.IsValid)
            {
                _context.Products.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
            
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                NotFound();
            }
            LoadCategories();
            var product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Product model)
        {
           
                _context.Products.Remove(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
          
        }

     }
}
