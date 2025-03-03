using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gitMARATHON.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Ідентифікатор користувача

        // Навігаційна властивість
        public virtual User User { get; set; }

        [Required]
        public int LectureId { get; set; } // Ідентифікатор лекції

        [ForeignKey("LectureId")]
        public Lecture Lecture { get; set; } // Навігаційна властивість

        [Range(0, 100)]
        public int Points { get; set; } // Оцінка
    }
}
