﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.DTO
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
