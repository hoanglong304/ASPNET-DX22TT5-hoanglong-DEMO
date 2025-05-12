using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace managershop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Size { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng từ 1 đến 100")]
        public int Quantity { get; set; }
    }
}
