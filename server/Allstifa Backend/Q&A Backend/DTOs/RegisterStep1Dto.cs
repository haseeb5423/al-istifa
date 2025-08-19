using System.ComponentModel.DataAnnotations;

namespace Q_A_Backend.DTOs
{
    public class RegisterStep1Dto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = "User";       // "User" or "Scholar"

        // Optional: You can add [Required] if you want to enforce for all, or handle conditional validation in controller
        public int? MaslakId { get; set; }
    }

}

