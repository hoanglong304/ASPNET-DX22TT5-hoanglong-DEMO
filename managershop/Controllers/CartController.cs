using managershop.Data;
using managershop.Models;
using managershop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // Hiển thị giỏ hàng
    public async Task<IActionResult> Index()
    {
        int userId = GetCurrentUserId();
        Console.WriteLine($"UserId hiện tại: {userId}");
        if (userId == 0)
        {
            // Chưa đăng nhập => chuyển đến login hoặc trả về rỗng
            return RedirectToAction("Login", "Account");
        }

        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToListAsync();

        return View(cartItems);
    }

    // Helper lấy UserId từ Claims
    private int GetCurrentUserId()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdStr, out int userId))
        {
            return userId;
        }
        return 0;
    }

    // Xoá item khỏi giỏ
    public async Task<IActionResult> Remove(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    // Thêm vào giỏ hàng - trả JSON để gọi ajax
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int size = 0)
    {
        int userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Json(new { success = false, message = "Vui lòng đăng nhập." });
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.Size == size);

        if (cartItem != null)
        {
            cartItem.Quantity++;
        }
        else
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Size = size,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
        }
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" });
    }


    [HttpGet]
    public async Task<IActionResult> GetCartSummary()
    {
        return ViewComponent("CartSummary");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity([FromBody] CartItemUpdateViewModel model)
    {
        try
        {
            if (model == null || model.Quantity < 1)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            var cartItem = await _context.CartItems.FindAsync(model.Id);
            if (cartItem == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
            }

            cartItem.Quantity = model.Quantity;
            await _context.SaveChangesAsync();

            var itemTotal = cartItem.Quantity * cartItem.Product.Price;

            return Ok(new
            {
                success = true,
                itemTotal = itemTotal
            });
        }
        catch (Exception ex)
        {
            // Log ex.Message nếu cần
            return StatusCode(500, new { success = false, message = "Lỗi server: " + ex.Message });
        }
    }

}
