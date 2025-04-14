using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryShoppingCartItemDto
    {
        [Required(ErrorMessage = "Grocery item reference is required")]
        public int GroceryItemId { get; set; }
        [Required(ErrorMessage = "Grocery item quantity should be more than zero")]
        public int Quantity { get; set; } = 0;
    }
}
