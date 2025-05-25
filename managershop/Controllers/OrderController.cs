using managershop.Data;
using managershop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class OrderController : Controller
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Lấy giỏ hàng của user từ CSDL
        var cartItems = _context.CartItems
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToList();

        if (!cartItems.Any())
        {
            TempData["Error"] = "Giỏ hàng trống.";
            return RedirectToAction("Index", "Cart");
        }

        // Tính tổng tiền
        decimal total = cartItems.Sum(c => c.Product.Price * c.Quantity);

        // Tạo đơn hàng
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Total = total,
            Status = "Đang xử lý"
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        // Tạo các dòng chi tiết đơn hàng
        foreach (var item in cartItems)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            };
            _context.OrderDetail.Add(orderDetail);
        }

        // Xóa giỏ hàng sau khi đặt hàng
        _context.CartItems.RemoveRange(cartItems);

        _context.SaveChanges();

        TempData["Success"] = "Đặt hàng thành công!";
        // Chuyển về trang chủ kèm query string báo đã đặt thành công
        return RedirectToAction("Index", "Home", new { ordered = true });
    }

    public IActionResult Details(int id)
    {
        var order = _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
    public IActionResult Index()
    {
        // Ép kiểu từ string → int
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var orders = _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }
}
