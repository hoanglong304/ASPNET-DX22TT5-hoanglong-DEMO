namespace managershop.ViewModels
{
    public class CartItemUpdateViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class CartUpdateViewModel
    {
        public List<CartItemUpdateViewModel> CartItems { get; set; } = new();
    }
    

}
