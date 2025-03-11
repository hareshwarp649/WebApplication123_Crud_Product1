using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApplication123.Infrastructure.IRepository;
using WebApplication123.Infrastructure.Repository;
using WebApplication123.Models;
using WebApplication123.ViewModel;


namespace WebApplication123.Areas.Admin.Controllers
{
    [Area("Admin")]
    

    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnvironment; // image file pickup karne ke liye 


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }
        #region APICALL

        [HttpGet]
        public IActionResult AllProducts()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // Ensure image URL is never null
            //foreach (var product in products)
            //{
            //    if (string.IsNullOrEmpty(product.ImageUrl))
            //    {
            //        product.ImageUrl = "/ProductImage/default.jpg"; // Set default image if null
            //    }
            //}

            return Json(new { data = products });
        }
        #endregion

        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitOfWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };


            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitOfWork.Product.GetT(x => x.Id == id);
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate( ProductVM vm, IFormFile? file)
        {
            //vm.Product.ImageUrl = null;
            //IFormFile formFile = file;
            if (ModelState.IsValid)
            {

                var existingProduct = _unitOfWork.Product.GetT(x => x.Name == vm.Product.Name && (vm.Product.Id == 0 || x.Id != vm.Product.Id));
                if (existingProduct != null)
                {

                    ModelState.AddModelError("Name", "A product with the same name already exists Please Update another Product name.");
                    return View(vm);
                }

                
                string fileName = String.Empty;
                if (file != null && file.Length > 0)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //vm.Product.ImageUrl = @"\ProductImage\" + fileName;
                    vm.Product.ImageUrl = "/ProductImage/"  + fileName;
                }
                else if (vm.Product.Id != 0) // Keep existing image if updating
                {
                    var existiProduct = _unitOfWork.Product.GetT(x => x.Id == vm.Product.Id);
                    if (existiProduct != null)
                    {
                        vm.Product.ImageUrl = existiProduct.ImageUrl; // Preserve existing image
                    }
                }
                else
                {
                    //vm.Product.ImageUrl = @"\ProductImage\default.jpg";

                     vm.Product.ImageUrl = "/ProductImage/default.jpg";
                }


                if (vm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(vm.Product);
                    TempData["success"] = "Product Created Done!";
                }
                else
                {
                    _unitOfWork.Product.Update(vm.Product);
                    TempData["success"] = "Product Update Done !";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }

        #region DeleteAPICALL
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var product = _unitOfWork.Product.GetT(x => x.Id == id);
            //if (product == null)
            //{
            //    return Json(new { Success = false, Error = "Error in Fenching Data" });
            //}
            //else
            //{

            //      var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            //        if (System.IO.File.Exists(oldImagePath))
            //        {
            //            System.IO.File.Delete(oldImagePath);
            //        }

            //    _unitOfWork.Product.Delete(product);
            //    _unitOfWork.Save();
            //    return Json(new { Success = true, message = "Product Delete" });
            //}

            var product = _unitOfWork.Product.GetT(x => x.Id == id);
            if (product == null)
            {
                return Json(new { Success = false, Error = "Error in Fetching Data" });
            }

            try
            {
                // ✅ Step 1: Check if ImageUrl is not null or empty
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    string oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

                    // ✅ Step 2: Log file path for debugging
                    Console.WriteLine("Deleting file: " + oldImagePath);

                    // ✅ Step 3: Check if file exists before deleting
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // ✅ Step 4: Delete product from database
                _unitOfWork.Product.Delete(product);
                _unitOfWork.Save();

                return Json(new { Success = true, Message = "Product Deleted Successfully!" });
            }
            catch (Exception ex)
            {
                // ✅ Step 5: Log the exception for debugging
                Console.WriteLine("Error Deleting Product: " + ex.Message);
                return Json(new { Success = false, Error = "Error in Deleting Product: " + ex.Message });
            }
        }
        #endregion





    }
}
