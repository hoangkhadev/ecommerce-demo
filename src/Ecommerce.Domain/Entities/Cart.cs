namespace Ecommerce.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? AppliedCouponCode { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
    }
}
