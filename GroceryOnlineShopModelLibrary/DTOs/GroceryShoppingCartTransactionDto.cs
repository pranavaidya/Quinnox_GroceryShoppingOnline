using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryShoppingCartTransactionDto
    {
        [Required(ErrorMessage = "Customer reference is required")]
        public int CustomerId { get; set; } = 0;
        [Required(ErrorMessage = "Grocery item reference is required")]
        public int GroceryItemId { get; set; }
        [Required(ErrorMessage = "Grocery item quantity should be more than zero")]
        public int QuantityChange { get; set; } = 0;
    }
}
