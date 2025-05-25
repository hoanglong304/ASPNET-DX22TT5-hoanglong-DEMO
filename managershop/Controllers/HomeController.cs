using managershop.Data; // Thêm namespace chứa DbContext
using managershop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace managershop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;  // Tiêm DbContext vào Controller

        // Constructor để tiêm ILogger và AppDbContext vào controller
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;  // Lưu đối tượng AppDbContext
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            IQueryable<Product> products = _context.Products;

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            var productList = await products.ToListAsync();

            return PartialView("_ProductListPartial", productList);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();  // Entity Framework sẽ tự xử lý giá trị NULL

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
