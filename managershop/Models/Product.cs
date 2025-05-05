using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace managershop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không hợp lệ.")]
        public int? Quantity { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu.")]
        public int? BrandId { get; set; }

        // Điều hướng (navigation)
        public Brand? Brand { get; set; }
        public Category? Category { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
