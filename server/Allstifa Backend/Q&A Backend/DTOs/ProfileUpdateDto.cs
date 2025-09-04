using Microsoft.AspNetCore.Http; // Add this using directive

namespace Q_A_Backend.DTOs
{
    public class ProfileUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public int? MaslakId { get; set; }
        public bool? IsVerified { get; set; }
        public string? SocialMediaLink { get; set; } // Optional social media link for proof
        
        // Add IFormFile properties for the uploads
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? ProofFile { get; set; }

        // These properties will be set by the controller after saving the files
        // and are not part of the form binding.
        public string? ProfileImagePath { get; set; }
        public string? ProofFilePath { get; set; }
    }
}