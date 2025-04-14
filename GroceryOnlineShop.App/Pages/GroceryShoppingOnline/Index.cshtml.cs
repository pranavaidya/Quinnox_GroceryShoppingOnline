using GroceryOnlineShopModelLibrary.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCHS.MS.App.Services;

namespace GroceryOnlineShop.App.Pages.GroceryShoppingOnline
{
    public class IndexModel : PageModel
    {
        private readonly ApiRequestProvider _apiRequest;

        [BindProperty]
        public List<GroceryItemDto> GroceryItemsAvailableToPurchase { get; set; } = new();
        [BindProperty]
        public int CustomerId { get; set; } = 0;
        [BindProperty]
        public GroceryShoppingCartDto? GroceryShoppingCart { get; private set; }

        public IndexModel(ApiRequestProvider apiRequest)
        {
            _apiRequest = apiRequest;
        }

        public IActionResult OnGet(int customerId)
        {
            CustomerId = customerId;
            LoadPrerequisites();
            return Page();
        }

        public IActionResult OnPost(int customerId)
        {
            return RedirectToPage("/GroceryShoppingOnline/Checkout", new { customerId = customerId });
        }

        public IActionResult OnPostIncreaseQuantity(int itemId, int customerId)
        {
            var shoppingCartTransaction = new GroceryShoppingCartTransactionDto
            {
                CustomerId = customerId,
                GroceryItemId = itemId,
                QuantityChange = 1
            };

            var shoppingCartResponse = _apiRequest.RequestPost<GroceryShoppingCartTransactionDto, GeneralResponseDto<GroceryShoppingCartDto>>("api/groceryshop/basket", shoppingCartTransaction).GetAwaiter().GetResult();
            if (shoppingCartResponse != null && shoppingCartResponse.Success)
                GroceryShoppingCart = shoppingCartResponse.Value!;

            return RedirectToPage("/GroceryShoppingOnline/Index", new { customerId = customerId });
        }

        public IActionResult OnPostDecreaseQuantity(int itemId, int customerId)
        {
            var shoppingCartTransaction = new GroceryShoppingCartTransactionDto
            {
                CustomerId = customerId,
                GroceryItemId = itemId,
                QuantityChange = -1
            };

            var shoppingCartResponse = _apiRequest.RequestPost<GroceryShoppingCartTransactionDto, GeneralResponseDto<GroceryShoppingCartDto>>("api/groceryshop/basket", shoppingCartTransaction).GetAwaiter().GetResult();
            if (shoppingCartResponse != null && shoppingCartResponse.Success)
                GroceryShoppingCart = shoppingCartResponse.Value!;

            return RedirectToPage("/GroceryShoppingOnline/Index", new { customerId = customerId });
        }

        private void LoadPrerequisites()
        {
            var customers = _apiRequest.Request<GeneralResponseDto<List<CustomerDto>>>("api/groceryshop/customers").GetAwaiter().GetResult();
            var itemsResponse = _apiRequest.Request<GeneralResponseDto<List<GroceryItemDto>>>("api/groceryshop/items").GetAwaiter().GetResult();
            var shoppingCartResponse = _apiRequest.Request<GeneralResponseDto<GroceryShoppingCartDto>>($"api/groceryshop/basket/{CustomerId}").GetAwaiter().GetResult();

            if (itemsResponse == null || !itemsResponse.Success || customers == null ||
                !customers.Success || shoppingCartResponse == null || !shoppingCartResponse.Success)
                return;

            var selectedCustomer = (customers!.Value!).FirstOrDefault(cr => cr.Id == CustomerId);
            ViewData["CustomerName"] = selectedCustomer!.Name;
            GroceryItemsAvailableToPurchase = itemsResponse!.Value!;
            GroceryShoppingCart = shoppingCartResponse.Value!;
        }
    }
}
