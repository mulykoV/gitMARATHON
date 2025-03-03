using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace gitMARATHON.Models
{
    public class Lecture
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public string VideoPath { get; set; } // Шлях до збереженого відеофайлу

        public string PracticalTask { get; set; }
    }
}
