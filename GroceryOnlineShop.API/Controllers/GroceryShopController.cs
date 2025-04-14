using GroceryOnlineShop.API.Database;
using GroceryOnlineShopModelLibrary.DTOs;
using GroceryOnlineShopModelLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroceryOnlineShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroceryShopController : ControllerBase
    {
        private readonly GroceryShopPurchaseOrderProcessor _gsManager;
        public GroceryShopController(GroceryShopPurchaseOrderProcessor gsManager)
        {
            _gsManager = gsManager;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetGroceryItems()
        {
            var response = new GeneralResponseDto<List<GroceryItemDto>>().Content(_gsManager.GroceryItems);
            return Ok(await Task.FromResult(response));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var response = new GeneralResponseDto<List<CustomerDto>>().Content(_gsManager.Customers);
            return Ok(await Task.FromResult(response));
        }

        [HttpGet("basket/{customerId}")]
        public async Task<IActionResult> GetBasket(int customerId)
        {
            var order = _gsManager.GetGroceryShoppingCart(customerId);
            var result = new GeneralResponseDto<GroceryShoppingCartDto>().Content(order);
            return Ok(await Task.FromResult(result));
        }

        [HttpPost("basket")]
        public async Task<IActionResult> PostToBasket([FromBody] GroceryShoppingCartTransactionDto shoppingCartTransaction)
        {
            _gsManager.ManageOrder(shoppingCartTransaction);
            var order = _gsManager.GetGroceryShoppingCart(shoppingCartTransaction.CustomerId);
            var result = new GeneralResponseDto<GroceryShoppingCartDto>().Content(order);
            return Ok(await Task.FromResult(result));
        }

        [HttpGet("checkout/{customerId}")]
        public IActionResult GetOrderToCheckout(int customerId)
        {
            var checkOutOrder = _gsManager.GetGroceryOrderDetails(customerId);
            return Ok(new GeneralResponseDto<GroceryOrderCheckoutDto>().Content(checkOutOrder));
        }

        [HttpPut("recalculate/{orderId}")]
        public IActionResult PutRecalculateOrder(int orderId)
        {
            _gsManager.RecalculateOrder(orderId);
            var customerRef = _gsManager.GetCustomerRefOfGroceryOrder(orderId);
            var checkOutOrder = _gsManager.GetGroceryOrderDetails(customerRef);
            return Ok(new GeneralResponseDto<GroceryOrderCheckoutDto>().Content(checkOutOrder));
        }

        [HttpPut("checkout/{orderId}")]
        public IActionResult PutCheckoutOrder(int orderId)
        {
            _gsManager.CheckoutOrder(orderId);
            return Ok(new GeneralResponseDto<string>().Content("success"));
        }

        [HttpGet("confirmedorder/{orderId}")]
        public IActionResult GetConfirmedOrderDetails(int orderId)
        {
            var checkOutOrder = _gsManager.GetConfirmedOrder(orderId);
            return Ok(new GeneralResponseDto<GroceryOrderCheckoutDto>().Content(checkOutOrder));
        }
    }
}
