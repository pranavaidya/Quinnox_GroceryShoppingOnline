using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.Models
{
    public class GroceryOrder
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; } = 0;
        public int NoOfItems { get; set; } = 0;
        public double TotalAmount { get; set; } = 0;
        public bool LoyaltyMembershipApplicable { get; set; } = false;
        public bool OrderCheckedOut { get; set; } = false;
    }
}
