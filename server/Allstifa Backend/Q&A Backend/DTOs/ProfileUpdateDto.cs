namespace Q_A_Backend.DTOs
{
    public class ProfileUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImagePath { get; set; }
        public int? MaslakId { get; set; }
        public bool? IsVerified { get; set; } 
        public string? ProofFilePath { get; set; }
        public string? SocialMediaLink { get; set; } // New: Social media link for proof
    }
}