using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Q_A_Backend.DTOs
{
    public class RegisterStep2Dto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }                 // Returned from Step 1 (or extract from JWT)
        public string? Bio { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? ProofFile { get; set; }        // Only for scholars
        public string? SocialMediaLink { get; set; }     // New: Social media link for proof
    }

}
