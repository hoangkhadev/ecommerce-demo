namespace Ecommerce.Domain.Entities
{
    public enum DisCountType
    {
        PERCENTAGE,
        FIXED_AMOUNT
    }

    public class Coupon
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public required string DiscountType { get; set; }
        public decimal Value { get; set; }
        public decimal MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
    }
}
