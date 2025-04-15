using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.EF
{
    public class Badges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgeID { get; set; }  // Khóa chính, tự tăng

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }  // Tên huy hiệu

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }  // Mô tả huy hiệu

        // Navigation Property - Một huy hiệu có thể được nhiều người dùng nhận
        public virtual ICollection<UserBadges> UserBadges { get; set; }

        public Badges()
        {
            UserBadges = new HashSet<UserBadges>();
        }
    }
}
