namespace Q_A_Backend.DTOs
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? Bio { get; set; }
        public string? ProfileImagePath { get; set; }
        public int? MaslakId { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? ProofFilePath { get; set; }
        public string? SocialMediaLink { get; set; } // New: Social media link for proof
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
