using managershop.Data;
using managershop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace managershop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                switch (category.ToLower())
                {
                    case "nam":
                        products = products.Where(p => p.CategoryId == 1);
                        break;
                    case "nu":
                        products = products.Where(p => p.CategoryId == 2);
                        break;
                    case "all":
                        products = _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Brand)
                        .AsQueryable();
                        break;
                }
            }

            return View(await products.ToListAsync());
        }

    }
}
