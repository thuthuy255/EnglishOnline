namespace Model.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Questions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionID { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        public string QuestionType { get; set; }

        // Navigation Property - Câu hỏi thuộc một bài học
        public virtual Lessons Lesson { get; set; }

        // Navigation Property - Một câu hỏi có nhiều câu trả lời
        public virtual ICollection<Answers> Answers { get; set; }

        public Questions()
        {
            Answers = new HashSet<Answers>();
        }
    }
}
