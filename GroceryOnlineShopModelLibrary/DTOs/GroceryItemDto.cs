using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryItemDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Grocery item title is required")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Grocery item description is required")]
        public string? Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Grocery item price cannot be zero")]
        public double Price { get; set; } = 0;
        public int AvailableQuantity { get; set; } = 0;
        public bool QuantityAdjustable { get; set; } = true;
    }
}
