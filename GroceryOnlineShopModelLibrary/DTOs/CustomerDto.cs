using System.ComponentModel.DataAnnotations;

namespace GroceryOnlineShopModelLibrary.DTOs
{
    public class CustomerDto
    {
        [Key]
        public int Id { get; set; } = 0;
        [Required(ErrorMessage = "Customer name is mandatory")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email-Id is mandatory")]
        public string EmailId { get; set; } = string.Empty;
        public bool LoyaltyMembership { get; set; } = false;
    }
}
