using ShoppingCartService.DTOs;
using ShoppingCartService.Models;

namespace ShoppingCartService.Mapping
{
    public static class CreateDTOToOrder
    {
        public static Order MapToOrder(this AddToShoppingCartDTO addToShoppingCartDTO)
        {
            return new Order
            {
                UserId = addToShoppingCartDTO.PostedBy,
                ItemId = addToShoppingCartDTO.ItemId
            };
        }
    }
}
