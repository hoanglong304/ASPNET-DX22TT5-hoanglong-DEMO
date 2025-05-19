using managershop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public CartSummaryViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        int userId = 0;
        if (User.Identity.IsAuthenticated)
        {
            userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
        var count = await _context.CartItems.Where(c => c.UserId == userId).SumAsync(c => (int?)c.Quantity) ?? 0;

        return View(count);
    }
}
