using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class Lessons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonID { get; set; }

        [Required]
        [MaxLength(255)]  // Đảm bảo tên bài học không quá dài
        public string LessonName { get; set; }

        [Required]
        public string LessonContent { get; set; }

        [Required]
        [MaxLength(50)] // Giới hạn độ dài của LessonType
        public string LessonType { get; set; }

        [Required]
        [Range(1, 5)] // Kiểm tra mức độ khó từ 1 đến 5
        [Column(TypeName = "int")]
        public int DifficultyLevel { get; set; }

        [Required]
        [Range(0, int.MaxValue)] // Kiểm tra điểm số không âm
        [Column(TypeName = "int")]
        public int Score { get; set; }

        // Navigation Properties
        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<UserProgress> UserProgresses { get; set; }

        // Sửa kiểu TopicID từ int thành Guid
        [ForeignKey("Topic")]
        public Guid TopicID { get; set; }

        public virtual Topic Topic { get; set; }

        // Constructor để khởi tạo các collection
        public Lessons()
        {
            Questions = new HashSet<Questions>();
            UserProgresses = new HashSet<UserProgress>();
        }
    }
}
