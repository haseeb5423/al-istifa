﻿using System.ComponentModel.DataAnnotations;

namespace Q_A_Backend.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress (ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";
    }
}
