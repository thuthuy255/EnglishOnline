using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class UserBadges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserBadgeID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Badge")]
        public int BadgeID { get; set; }

        public DateTime AwardedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Users User { get; set; }
        public virtual Badges Badge { get; set; }
    }
}
