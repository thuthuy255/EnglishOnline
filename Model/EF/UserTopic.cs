using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class UserTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTopicID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public Guid TopicID { get; set; }

        [Required]
        public bool Completed { get; set; }

        // Navigation Properties
        [ForeignKey("UserID")]
        public virtual Users User { get; set; }

        [ForeignKey("TopicID")]
        public virtual Topic Topic { get; set; }

        // Constructor mặc định
        public UserTopic()
        {
            Completed = false; // mặc định là chưa hoàn thành
        }
    }
}
