namespace ShoppingCartService
{
    public class CustomKeyGenerator
    {
        public static string GenerateKey(int userId, int itemId)
        {
            return userId.ToString() + ":" + itemId.ToString();
        }
    }
}
