namespace Ecommerce.Domain.Entities
{
    public enum UserRole
    {
        User,
        Admin
    }

    public enum RankEnum
    {
        Silver,
        Gold,
        Dinamond
    }
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = UserRole.User.ToString();
        public Cart? Cart { get; set; } = null!;
        public int LoyaltyPoints { get; set; }
        public int TotalAccumulatedPoints { get; set; }
        public string Rank { get; set; } = nameof(RankEnum.Silver);
    }
}
