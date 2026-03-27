namespace Ecommerce.Domain.Entities
{
    public enum OrderStatus
    {
        Pending,
        Approved,
        Shipped,
        Completed,
        Cancelled
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = OrderStatus.Pending.ToString();
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}
