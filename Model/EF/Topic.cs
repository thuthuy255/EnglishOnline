    using Model.EF;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System;
    namespace Model.EF
    {
        public class Topic
        {
            [Key]
            public Guid TopicID { get; set; }

            [Required]
            [MaxLength(255)]
            public string TopicName { get; set; }

            public Guid? PrerequisiteTopicId { get; set; }

            [ForeignKey("PrerequisiteTopicId")]
            public virtual Topic PrerequisiteTopic { get; set; }

            public virtual ICollection<Lessons> Lessons { get; set; }
            public virtual ICollection<UserTopic> UserTopic { get; set; }

            public Topic()
            {
                Lessons = new HashSet<Lessons>();
            }
        }
    }
