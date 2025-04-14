using GroceryOnlineShopModelLibrary.DTOs;
using GroceryOnlineShopModelLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCHS.MS.App.Services;
using System.Reflection.Metadata.Ecma335;

namespace GroceryOnlineShop.App.Pages.GroceryShoppingOnline
{
    public class CheckoutModel : PageModel
    {
        private readonly ApiRequestProvider _apiRequest;

        [BindProperty]
        public int CustomerId { get; set; } = 0;
        [BindProperty]
        public GroceryOrderCheckoutDto? GroceryOrder { get; private set; }

        public CheckoutModel(ApiRequestProvider apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public IActionResult OnGet(int customerId)
        {
            LoadPrerequisites(customerId);
            return Page();
        }

        public IActionResult OnPostLoyaltyMembershipChanged(int custId, int orderId)
        {
            CustomerId = custId;
            var response = _apiRequest.RequestUpdate<GeneralResponseDto<GroceryOrderCheckoutDto>?>($"api/groceryshop/recalculate/{orderId}", null).GetAwaiter().GetResult();
            if (response != null && response.Success)
                GroceryOrder = response.Value;
            return RedirectToPage("/GroceryShoppingOnline/Checkout", new { customerId = CustomerId });
        }

        public IActionResult OnPostConfirmOrder(int orderId)
        {
            var response = _apiRequest.RequestUpdate<GeneralResponseDto<string>?>($"api/groceryshop/checkout/{orderId}", null).GetAwaiter().GetResult();
            if (response == null || !response.Success)
                return Page();
            return RedirectToPage("/GroceryShoppingOnline/OrderConfirmed", new { orderId = orderId });
        }

        private void LoadPrerequisites(int customerId)
        {
            var customers = _apiRequest.Request<GeneralResponseDto<List<CustomerDto>>>("api/groceryshop/customers").GetAwaiter().GetResult();
            if (customers == null || !customers.Success)
                return;

            CustomerId = customerId;
            var selectedCustomer = (customers!.Value!).FirstOrDefault(cr => cr.Id == CustomerId);
            ViewData["CustomerName"] = selectedCustomer!.Name;

            var orderResponse = _apiRequest.Request<GeneralResponseDto<GroceryOrderCheckoutDto>>($"api/groceryshop/checkout/{CustomerId}").GetAwaiter().GetResult();
            if (orderResponse == null || !orderResponse.Success)
                return;

            GroceryOrder = orderResponse!.Value!;
        }
    }
}
