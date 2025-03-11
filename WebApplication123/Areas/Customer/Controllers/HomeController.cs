using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication123.Infrastructure.IRepository;
using WebApplication123.Models;

namespace WebApplication123.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Product> products =_unitOfWork.Product.GetAll(includeProperties: "Category");

            return View(products);
        }

        [HttpGet]
        public IActionResult Details()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");

            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
