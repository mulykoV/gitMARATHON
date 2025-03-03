using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace gitMARATHON.Models
{
    public class PetProject
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Ідентифікатор користувача

        public string Name { get; set; }

        public string GitHubLink { get; set; } // Посилання на проєкт

        public bool IsUnlocked { get; set; } // Доступний після проходження всіх практичних задач
    }
}