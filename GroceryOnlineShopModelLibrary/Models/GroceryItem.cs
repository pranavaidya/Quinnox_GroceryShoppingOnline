using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.Models
{
    public class GroceryItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public int AvailableQuantity { get; set; } = 0;
        public bool QuantityAdjustable { get; set; } = true;
    }
}
