using System.ComponentModel.DataAnnotations;

namespace HspFrontEnd.Models
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{12,}$", ErrorMessage = "Minimum length 12 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
