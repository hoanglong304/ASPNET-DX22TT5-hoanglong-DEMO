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

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToListAsync();

            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            var model = new ProductCreateModel
            {
                // Khởi tạo danh sách danh mục và thương hiệu
                CategoryList = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),

                BrandList = _context.Brands.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }).ToList()
            };

            return View(model);
        }


        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateModel model)
        {
            if (model.ImageFile == null || model.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Vui lòng chọn ảnh sản phẩm.");
            }

            if (ModelState.IsValid)
            {

                // Xử lý ảnh nếu có
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    model.Product.ImageUrl = "/images/" + fileName;
                }

                // Cập nhật ngày tạo
                model.Product.CreatedAt = DateTime.Now;

                // Lưu vào database
                _context.Products.Add(model.Product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nạp lại danh sách nếu model không hợp lệ
            model.CategoryList = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            model.BrandList = _context.Brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            return View(model);
        }


    }
}
