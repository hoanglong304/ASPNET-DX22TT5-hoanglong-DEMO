namespace managershop.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
