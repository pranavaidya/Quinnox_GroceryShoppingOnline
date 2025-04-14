using GroceryOnlineShopModelLibrary.DTOs;
using GroceryOnlineShopModelLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCHS.MS.App.Services;

namespace GroceryOnlineShop.App.Pages.GroceryShoppingOnline
{
    public class OrderConfirmedModel : PageModel
    {
        private readonly ApiRequestProvider _apiRequest;

        [BindProperty]
        public int OrderId { get; set; } = 0;
        [BindProperty]
        public GroceryOrderCheckoutDto? GroceryOrder { get; private set; }

        public OrderConfirmedModel(ApiRequestProvider apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public IActionResult OnGet(int orderId) 
        {
            var customers = _apiRequest.Request<GeneralResponseDto<List<CustomerDto>>>("api/groceryshop/customers").GetAwaiter().GetResult();
            if (customers == null || !customers.Success)
                return Page();

            OrderId = orderId;

            var orderResponse = _apiRequest.Request<GeneralResponseDto<GroceryOrderCheckoutDto>>($"api/groceryshop/confirmedorder/{orderId}").GetAwaiter().GetResult();
            if (orderResponse == null || !orderResponse.Success)
                return Page();

            GroceryOrder = orderResponse!.Value!;
            var selectedCustomer = (customers!.Value!).FirstOrDefault(cr => cr.Id == GroceryOrder.CustomerId);
            ViewData["CustomerName"] = selectedCustomer!.Name;

            return Page();
        }
    }
}
