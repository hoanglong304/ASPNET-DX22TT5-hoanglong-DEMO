namespace managershop.Models
{
    public class CartItem
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Size { get; set; }
        public int Quantity { get; set; }
    }

}
