using AutoMapper;
using GroceryOnlineShopModelLibrary.DTOs;
using GroceryOnlineShopModelLibrary.Models;

namespace GroceryOnlineShop.API.Database
{
    public class GroceryShopPurchaseOrderProcessor
    {
        private readonly IMapper _mapper;
        private readonly GroceryShopDatabase _groceryDb;

        public GroceryShopPurchaseOrderProcessor(IMapper mapper, GroceryShopDatabase groceryDb)
        {
            _mapper = mapper;
            _groceryDb = groceryDb;
        }

        public void ManageOrder(GroceryShoppingCartTransactionDto shoppingCartTransaction)
        {
            var groceryOrderFound = _groceryDb.GroceryOrders.Find(go =>
                go.OrderDate.Date == DateTime.Now.Date
                && go.CustomerId == shoppingCartTransaction.CustomerId && !go.OrderCheckedOut);

            if (groceryOrderFound == null)
            {
                var customer = _groceryDb.Customers
                    .FirstOrDefault(cr => cr.Id == shoppingCartTransaction.CustomerId);

                var groceryOrder = new GroceryOrder
                {
                    CustomerId = shoppingCartTransaction.CustomerId,
                    OrderDate = DateTime.Now,
                    OrderCheckedOut = false,
                    LoyaltyMembershipApplicable = (customer != null && customer.LoyaltyMembership),
                    Id = _groceryDb.GroceryOrders.Count + 1
                };
                _groceryDb.GroceryOrders.Add(groceryOrder);

                groceryOrderFound = groceryOrder;
            }

            ManageOrderItems(groceryOrderFound, shoppingCartTransaction);
            ModifyOrder(groceryOrderFound!);
        }
        private void ManageOrderItems(GroceryOrder groceryOrder, GroceryShoppingCartTransactionDto shoppingCartTransaction)
        {
            var groceryOrderItemFound = _groceryDb.GroceryOrderItems.Find(goi =>
                   goi.GroceryOrderId == groceryOrder.Id
                   && goi.GroceryItemId == shoppingCartTransaction.GroceryItemId);

            if (groceryOrderItemFound == null)
            {
                var groceryOrderedItem = new GroceryOrderItem
                {
                    GroceryItemId = shoppingCartTransaction.GroceryItemId,
                    GroceryOrderId = groceryOrder.Id,
                    Quantity = shoppingCartTransaction.QuantityChange,
                    Id = _groceryDb.GroceryOrderItems.Count + 1
                };
                _groceryDb.GroceryOrderItems.Add(groceryOrderedItem);

                groceryOrderItemFound = groceryOrderedItem;
            }
            else
            {
                groceryOrderItemFound.Quantity += shoppingCartTransaction.QuantityChange;
                var item = _groceryDb.GroceryItems.Find(gi => gi.Id == groceryOrderItemFound.GroceryItemId);
                if (item != null)
                {
                    if (groceryOrderItemFound.Quantity < 0)
                        groceryOrderItemFound.Quantity = 0;
                    else if (groceryOrderItemFound.Quantity > item.AvailableQuantity)
                        groceryOrderItemFound.Quantity = item.AvailableQuantity;
                }
            }
        }
        private void ModifyOrder(GroceryOrder groceryOrder)
        {
            var orderInDetail =
                from co in _groceryDb.GroceryOrders
                join
                coi in _groceryDb.GroceryOrderItems
                on co.Id equals coi.GroceryOrderId
                join
                gi in _groceryDb.GroceryItems
                on coi.GroceryItemId equals gi.Id
                where co.Id == groceryOrder.Id
                select new
                {
                    OrderId = co.Id,
                    OrderItemId = coi.GroceryItemId,
                    OrderItemCost = GetCostAfterApplicableDeduction(co, (gi.Price * coi.Quantity))
                };

            foreach (var item in orderInDetail.ToList())
            {
                var groceryOrderedItem = _groceryDb.GroceryOrderItems.Find(oi => oi.GroceryItemId == item.OrderItemId && oi.GroceryOrderId == item.OrderId);
                if (groceryOrderedItem == null) continue;
                groceryOrderedItem.Cost = item.OrderItemCost;
            }

            groceryOrder.NoOfItems = orderInDetail.Count();
            groceryOrder.TotalAmount = orderInDetail.Sum(oid => oid.OrderItemCost);
            groceryOrder.TotalAmount += (groceryOrder.LoyaltyMembershipApplicable ? 5 : 0);
        }
        private double GetCostAfterApplicableDeduction(GroceryOrder custOrder, double primaryCost)
        {
            var defaultCost = primaryCost;
            if (custOrder.LoyaltyMembershipApplicable)
            {
                double deductionToApply = 0.2; // 20%
                double deductionValue = defaultCost * deductionToApply;
                defaultCost -= deductionValue;
            }
            return Math.Round(defaultCost, 2);
        }

        public void RecalculateOrder(int orderId)
        {
            var groceryOrderFound = _groceryDb.GroceryOrders.Find(go => go.Id == orderId && !go.OrderCheckedOut);

            if (groceryOrderFound == null)
                return;
            groceryOrderFound.LoyaltyMembershipApplicable = !groceryOrderFound.LoyaltyMembershipApplicable;
            ModifyOrder(groceryOrderFound);
        }
        public bool CheckoutOrder(int orderId)
        {
            var groceryOrderFound = _groceryDb.GroceryOrders.Find(go => go.Id == orderId && !go.OrderCheckedOut);

            if (groceryOrderFound == null)
                return false;

            groceryOrderFound.OrderCheckedOut = true;
            return true;
        }

        public int GetCustomerRefOfGroceryOrder(int orderId)
        {
            var groceryOrderFound = _groceryDb.GroceryOrders.Find(go => go.Id == orderId);
            return groceryOrderFound?.CustomerId ?? 0;
        }

        public GroceryOrderCheckoutDto? GetGroceryOrderDetails(int customerId, int orderId = 0, bool checkedOut = false)
        {
            var checkoutOrderDto =
               from co in _groceryDb.GroceryOrders
               join cr in _groceryDb.Customers
               on co.CustomerId equals cr.Id
               where co.CustomerId == customerId && co.OrderCheckedOut == checkedOut
                    && ((orderId > 0 && co.Id == orderId) || (orderId == 0 && co.OrderDate.Date.Equals(DateTime.Now.Date)))
               select new GroceryOrderCheckoutDto
               {
                   CustomerId = customerId,
                   GroceryOrderId = co.Id,
                   LoyaltyMembershipApplicable = co.LoyaltyMembershipApplicable,
                   NoOfItems = co.NoOfItems,
                   TotalAmount = co.TotalAmount,
                   OrderDate = co.OrderDate,
                   OrderItems = (
                        from coi in _groceryDb.GroceryOrderItems
                        join gi in _groceryDb.GroceryItems
                        on coi.GroceryItemId equals gi.Id
                        where coi.GroceryOrderId == co.Id
                        select new GroceryOrderCheckoutItemDto
                        {
                            GroceryItemId = coi.Id,
                            GroceryOrderId = coi.GroceryOrderId,
                            ItemName = gi.Title,
                            ItemDescription = gi.Description ?? string.Empty,
                            Quantity = coi.Quantity,
                            Cost = coi.Cost
                        }
                    ).ToList()
               };

            return checkoutOrderDto.SingleOrDefault();
        }
        public GroceryShoppingCartDto? GetGroceryShoppingCart(int customerId)
        {
            var shoppingCart = _groceryDb.GroceryOrders.FirstOrDefault(go => go.CustomerId == customerId
                && go.OrderDate.Date.Equals(DateTime.Now.Date) && !go.OrderCheckedOut);

            if (shoppingCart == null)
                return null;

            var shoppingCartDto = new GroceryShoppingCartDto
            {
                CustomerId = customerId,
                OrderDate = shoppingCart.OrderDate,
                NoOfItems = shoppingCart.NoOfItems,
                TotalAmount = shoppingCart.TotalAmount,
                CartItems = new()
            };

            var shoppingCartItems = _groceryDb.GroceryOrderItems
                .Where(goi => goi.GroceryOrderId == shoppingCart.Id);

            foreach (var shoppingCartItem in shoppingCartItems)
                shoppingCartDto.CartItems.Add(new GroceryShoppingCartItemDto()
                {
                    GroceryItemId = shoppingCartItem.GroceryItemId,
                    Quantity = shoppingCartItem.Quantity
                });

            return shoppingCartDto;
        }
        public GroceryOrderCheckoutDto? GetConfirmedOrder(int orderId)
        {
            var customerId = GetCustomerRefOfGroceryOrder(orderId);
            var groceryOrder = GetGroceryOrderDetails(customerId, orderId: orderId, checkedOut: true);
            return groceryOrder;
        }

        public List<CustomerDto> Customers => _mapper.Map<List<CustomerDto>>(_groceryDb.Customers);
        public List<GroceryItemDto> GroceryItems => _mapper.Map<List<GroceryItemDto>>(_groceryDb.GroceryItems);
    }
}
