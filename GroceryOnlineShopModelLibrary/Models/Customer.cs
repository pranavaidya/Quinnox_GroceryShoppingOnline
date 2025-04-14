using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public bool LoyaltyMembership { get; set; } = false;
    }
}
