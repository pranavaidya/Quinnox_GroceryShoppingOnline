using GroceryOnlineShopModelLibrary.Models;

namespace GroceryOnlineShop.API.Database
{
    public class GroceryShopDatabase
    {
        public List<GroceryItem> GroceryItems { get; set; }
        public List<GroceryOrder> GroceryOrders { get; set; }
        public List<GroceryOrderItem> GroceryOrderItems { get; set; }
        public List<Customer> Customers { get; set; }

        public GroceryShopDatabase()
        {
            GroceryItems = new List<GroceryItem>();
            GroceryOrders = new List<GroceryOrder>();
            GroceryOrderItems = new List<GroceryOrderItem>();
            Customers = new List<Customer>();
        }

        public async Task Seed()
        {
            if (Customers.Count > 0)
                return;

            Customers.Add(new Customer
            {
                Name = "Grocery-Shop-Customer-1st",
                EmailId = "customer.1st@someaccount.com",
                LoyaltyMembership = false,
                Id = 1
            });
            Customers.Add(new Customer
            {
                Name = "Grocery-Shop-Customer-2nd",
                EmailId = "customer.2nd@someaccount.com",
                LoyaltyMembership = true,
                Id = 2
            });

            GroceryItems.AddRange(new GroceryItem[]
            {
               new () {
                Title = "Milk",
                Description = "The best of cows",
                AvailableQuantity = 75,
                Price = 45,
                QuantityAdjustable = true,
                   Id=101 },
               new () {
                Title = "Bread",
                Description = "Easy toast",
                AvailableQuantity = 85,
                Price = 20,
                QuantityAdjustable = true,
               Id=102},
               new () {
                Title = "Eggs",
                Description = "Wild chicken",
                AvailableQuantity = 55,
                Price = 35,
                QuantityAdjustable = true,
                Id=103}
               //new () {
               // Title = "EasyGrocery Loyalty Membership",
               // AvailableQuantity = 1,
               // Price = 20,
               // QuantityAdjustable = false}
            });

            await Task.CompletedTask;
        }
    }
}
