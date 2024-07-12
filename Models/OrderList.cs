using Redis.OM.Modeling;

namespace ShoppingCartService.Models
{
    [Document]
    public class OrderList
    {
        [Indexed]
        public int UserId
        {
            get; set;
        }
        public int ItemId
        {
            get; set;
        }
    }
}
