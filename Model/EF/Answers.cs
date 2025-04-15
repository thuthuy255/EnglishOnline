using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class Answers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerID { get; set; }

        [Required]
        public string AnswerText { get; set; }

        [Required]
        public bool IsCorrect { get; set; } // Xác định đúng/sai

        [MaxLength(500)]
        public string ImagePath { get; set; } // Đường dẫn ảnh
        [Required]
        public int QuestionID { get; set; } 

        // Navigation Property - Liên kết đến bảng Questions
        [ForeignKey("QuestionID")]
        public virtual Questions Question { get; set; }
    }
}
