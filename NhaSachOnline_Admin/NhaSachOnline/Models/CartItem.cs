namespace NhaSachOnline.Models
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    };
}
