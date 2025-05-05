namespace managershop.Models
{
    public class ProductSize
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Size { get; set; }
        public int Quantity { get; set; }
    }

}
