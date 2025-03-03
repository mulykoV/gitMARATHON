using System.ComponentModel.DataAnnotations;

namespace gitMARATHON.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsCompleted { get; set; } = false;
    }
}
