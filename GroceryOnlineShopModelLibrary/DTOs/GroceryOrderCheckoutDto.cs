using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class GroceryOrderCheckoutDto
    {
        public int GroceryOrderId { get; set; } = 0;
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; } = 0;
        public List<GroceryOrderCheckoutItemDto> OrderItems { get; set; } = new();
        public int NoOfItems { get; set; } = 0;
        public double TotalAmount { get; set; } = 0;
        public bool LoyaltyMembershipApplicable { get; set; } = false;
    }
}
