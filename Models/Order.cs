using Redis.OM.Modeling;

namespace ShoppingCartService.Models
{
    [Document]
    public class Order
    {
        [RedisIdField]
        public string OrderId { get; set; }

        [Indexed]
        public int UserId { get; set; }
        public int ItemId { get; set; }
    }
}
