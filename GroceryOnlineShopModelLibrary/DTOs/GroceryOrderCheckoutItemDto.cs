namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryOrderCheckoutItemDto
    {
        public int GroceryOrderId { get; set; } = 0;
        public int? GroceryItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public double Cost { get; set; } = 0;
    }
}
