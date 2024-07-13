namespace ShoppingCartService.DTOs
{
    public record class AddToShoppingCartDTO
    {
        public required int ItemId { get; set; }
        public required int PostedBy { get; set; }
    }
}
