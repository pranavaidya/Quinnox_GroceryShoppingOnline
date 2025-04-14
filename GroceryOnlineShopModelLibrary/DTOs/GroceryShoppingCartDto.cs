using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryShoppingCartDto
    {
        [Required(ErrorMessage = "Order date cannot be empty")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Customer reference is required")]
        public int CustomerId { get; set; } = 0;

        public List<GroceryShoppingCartItemDto> CartItems { get; set; } = new();

        public int NoOfItems { get; set; } = 0;
        public double TotalAmount { get; set; } = 0;
    }
}
