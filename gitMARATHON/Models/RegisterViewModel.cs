using System.ComponentModel.DataAnnotations;

namespace gitMARATHON.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Це опційне поле, яке дозволяє ввести "секретну фразу" для створення адміна
        public string? SecretPhrase { get; set; }
    }
}
