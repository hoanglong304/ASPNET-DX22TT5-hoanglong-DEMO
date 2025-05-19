using managershop.Data;
using managershop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProductManagerController : Controller
{
    private readonly AppDbContext _context;

    public ProductManagerController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string category)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsQueryable();

        return View(await products.ToListAsync());
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
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.Product.ImageUrl = "/images/product/" + fileName;
            }

            // Cập nhật ngày tạo
            model.Product.CreatedAt = DateTime.Now;

            // Lưu vào database
            _context.Products.Add(model.Product);
            await _context.SaveChangesAsync();

            // Thông báo tạo thành công
            TempData["SuccessMessage"] = "Tạo sản phẩm mới thành công!";
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


    // GET: Product/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var model = new ProductEditModel
        {
            Product = product,
            BrandList = _context.Brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList(),
            CategoryList = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return View(model);
    }


    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductEditModel model)
    {
        if (id != model.Product.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct == null)
                return NotFound();

            // Nếu không chọn ảnh mới, giữ nguyên ảnh cũ
            if (model.ImageFile == null || model.ImageFile.Length == 0)
            {
                model.Product.ImageUrl = existingProduct.ImageUrl;
            }
            else
            {
                // Xử lý ảnh mới
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "product");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                model.Product.ImageUrl = "/images/product/" + fileName;
            }

            // Cập nhật sản phẩm
            _context.Update(model.Product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "sửa sản phẩm thành công!";
            return RedirectToAction(nameof(Details), new { id = model.Product.Id });
        }

        // Reload lists
        model.BrandList = _context.Brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToList();
        model.CategoryList = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        return View(model);
    }

    // Delete GET: Product/Delete/5   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Xoá ảnh nếu có
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Đã xoá sản phẩm thành công!";
        return RedirectToAction(nameof(Index));

    }
}
