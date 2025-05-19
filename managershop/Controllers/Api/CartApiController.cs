using managershop.Data;
using managershop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // cần để dùng Include và FirstOrDefaultAsync
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CartApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("UpdateQuantity")]
    public async Task<IActionResult> UpdateQuantity([FromBody] CartItemUpdateViewModel model)
    {
        var item = await _context.CartItems
            .Include(c => c.Product)  // Load sản phẩm kèm theo
            .FirstOrDefaultAsync(c => c.Id == model.Id);

        if (item == null) return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ" });

        item.Quantity = model.Quantity;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            itemTotal = item.Quantity * item.Product.Price,
            message = "Cập nhật thành công"
        });
    }
}
