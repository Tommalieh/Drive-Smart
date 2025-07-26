namespace Drivia.Entities;

public class UserRefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}