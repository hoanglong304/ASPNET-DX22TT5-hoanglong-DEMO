using managershop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProductEditModel
{
    public Product Product { get; set; }

    public IFormFile? ImageFile { get; set; }

    public List<SelectListItem> BrandList { get; set; } = new();
    public List<SelectListItem> CategoryList { get; set; } = new();
}
