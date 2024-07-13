using Redis.OM.Modeling;

namespace ShoppingCartService.DTOs
{
    public class RemoveFromShoppingCartDTO
    {
        public int ItemId { get; set; }
        public int PostedBy { get; set; }
    }
}
