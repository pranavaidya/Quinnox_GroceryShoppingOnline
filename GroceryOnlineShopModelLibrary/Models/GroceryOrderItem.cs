using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.Models
{
    public class GroceryOrderItem
    {
        [Key]
        public int Id { get; set; }
        public int GroceryOrderId { get; set; } = 0;
        public int GroceryItemId { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public double Cost { get; set; } = 0;
    }
}
