using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace gitMARATHON.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }  // Унікальне ім'я
        public bool IsAdmin { get; set; }
    }
}
