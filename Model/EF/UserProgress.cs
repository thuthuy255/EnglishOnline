using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class UserProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgressID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Lesson")]
        public int LessonID { get; set; }

        public int Score { get; set; } = 0;
        public bool Completed { get; set; } = false;

        // Navigation Properties
        public virtual Users User { get; set; }
        public virtual Lessons Lesson { get; set; }
    }
}
