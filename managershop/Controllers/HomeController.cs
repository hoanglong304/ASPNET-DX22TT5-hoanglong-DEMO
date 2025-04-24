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

        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var products = _context.Products.ToList(); // Truy vấn tất cả sản phẩm từ cơ sở dữ liệu

            // Truyền danh sách sản phẩm vào View
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
