using AutoMapper;
using GroceryOnlineShopModelLibrary.DTOs;
using GroceryOnlineShopModelLibrary.Models;

namespace GroceryOnlineShop.API.EntityAutomappings
{
    public class MappingProfile
    {
        public static MapperConfiguration RegisterMapping()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CustomerDto, Customer>().ReverseMap();
                config.CreateMap<GroceryItemDto, GroceryItem>().ReverseMap();
                //config.CreateMap<GroceryShoppingCartDto, GroceryOrder>().ReverseMap();
                //config.CreateMap<GroceryShoppingCartItemDto, GroceryOrderItem>().ReverseMap();
            });
        }
    }
}
