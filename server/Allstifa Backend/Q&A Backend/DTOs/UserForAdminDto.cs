using Q_A_Backend.Models;

namespace Q_A_Backend.DTOs
{
    public class UserForAdminDto
    {
         public Guid Id { get; set; }                      // Primary Key (GUID)
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = "User";        // User, Scholar, Admin
            public string? Bio { get; set; }
            public string? ProfileImagePath { get; set; }
            public int? MaslakId { get; set; }                // Nullable for non-scholars
            public bool IsVerified { get; set; } = false;     // Scholars only
            public string? ProofFilePath { get; set; }
            public string? SocialMediaLink { get; set; }      // New: Social media link for proof
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}