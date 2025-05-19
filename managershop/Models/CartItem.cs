using managershop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    [Required]
    public int Size { get; set; }

    [Required]
    [Range(1, 100)]
    public int Quantity { get; set; }
}
