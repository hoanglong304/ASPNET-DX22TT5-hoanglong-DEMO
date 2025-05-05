using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace managershop.Models
{
    public class ProductCreateModel
    {
        public Product Product { get; set; } = new Product();

        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> BrandList { get; set; } = new List<SelectListItem>();

        public IFormFile? ImageFile { get; set; } // Dùng để nhận ảnh upload từ form
    }
}
