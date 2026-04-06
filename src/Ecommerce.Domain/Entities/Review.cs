namespace Ecommerce.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public User User { get; set; } = null!;
        public Order Order { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
