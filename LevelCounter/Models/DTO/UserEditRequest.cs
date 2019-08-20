using System.ComponentModel.DataAnnotations;

namespace LevelCounter.Models.DTO
{
    public class UserEditRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "The field Password must be a string with a minimum length of '{2}' and a maximum length of '{1}'.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]*",
            ErrorMessage = "Your password must contain at least one upper case letter, one number and no special characters.")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "The field Password must be a string with a minimum length of '{2}' and a maximum length of '{1}'.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]*",
            ErrorMessage = "Your password must contain at least one upper case letter, one number and no special characters.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The two password do not match.")]
        public string VerifyNewPassword { get; set; }

        [Url]
        public string AvatarUrl { get; set; }
    }
}
