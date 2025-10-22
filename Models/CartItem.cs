namespace Models
{
    public class CartItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
